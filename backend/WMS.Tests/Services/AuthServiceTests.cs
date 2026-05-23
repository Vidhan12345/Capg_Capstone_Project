using Microsoft.Extensions.Configuration;
using Moq;
using WMS.Application.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Services;
using Xunit;

namespace WMS.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "WMS_SuperSecretKey_2026_MustBeAtLeast32CharactersLong!",
                    ["Jwt:Issuer"] = "WMS.API",
                    ["Jwt:Audience"] = "WMS.Frontend",
                    ["Jwt:ExpiryInMinutes"] = "60",
                    ["Jwt:RefreshTokenExpiryInDays"] = "7"
                }!)
                .Build();

            _service = new AuthService(_unitOfWorkMock.Object, _configuration);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidUsername_ThrowsUnauthorizedAccessException()
        {
            _unitOfWorkMock.Setup(u => u.UserLogins.GetByUsernameAsync("invalid")).ReturnsAsync((UserLogin?)null);

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(new LoginRequest { Username = "invalid", Password = "pass" }));
            Assert.Equal("Invalid username or password", ex.Message);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ThrowsUnauthorizedAccessException()
        {
            var userLogin = new UserLogin { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct") };
            _unitOfWorkMock.Setup(u => u.UserLogins.GetByUsernameAsync("admin")).ReturnsAsync(userLogin);

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(new LoginRequest { Username = "admin", Password = "wrong" }));
            Assert.Equal("Invalid username or password", ex.Message);
        }

        [Fact]
        public async Task LoginAsync_WithInactiveEmployee_ThrowsUnauthorizedAccessException()
        {
            var userLogin = new UserLogin { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), EmployeeId = 1 };
            _unitOfWorkMock.Setup(u => u.UserLogins.GetByUsernameAsync("admin")).ReturnsAsync(userLogin);
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(1)).ReturnsAsync(new Employee { EmployeeId = 1, IsActive = false });

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(new LoginRequest { Username = "admin", Password = "Admin@123" }));
            Assert.Equal("Employee account is inactive", ex.Message);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
        {
            var role = new Role { Name = "Admin" };
            var employee = new Employee { EmployeeId = 1, FirstName = "System", LastName = "Admin", Email = "admin@wms.com", IsActive = true, Role = role, EmployeeCode = "ADM001", DepartmentId = 1, RoleId = 1 };
            var userLogin = new UserLogin { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), EmployeeId = 1 };

            _unitOfWorkMock.Setup(u => u.UserLogins.GetByUsernameAsync("admin")).ReturnsAsync(userLogin);
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(1)).ReturnsAsync(employee);

            var result = await _service.LoginAsync(new LoginRequest { Username = "admin", Password = "Admin@123" });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.NotEmpty(result.RefreshToken);
            Assert.Equal("Admin", result.Role);
        }

        [Fact]
        public async Task LogoutAsync_ClearsRefreshToken()
        {
            var userLogin = new UserLogin { UserId = 1, RefreshToken = "some-token", RefreshTokenExpiry = DateTime.UtcNow.AddDays(1) };
            _unitOfWorkMock.Setup(u => u.UserLogins.GetByEmployeeIdAsync(1)).ReturnsAsync(userLogin);

            await _service.LogoutAsync(1);

            Assert.Null(userLogin.RefreshToken);
            Assert.Null(userLogin.RefreshTokenExpiry);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshTokenAsync_WithExpiredToken_ThrowsUnauthorizedAccessException()
        {
            var userLogin = new UserLogin { RefreshToken = "expired", RefreshTokenExpiry = DateTime.UtcNow.AddDays(-1) };
            _unitOfWorkMock.Setup(u => u.UserLogins.FindAsync(u => u.RefreshToken == "expired")).ReturnsAsync(new List<UserLogin> { userLogin });

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = "expired" }));
            Assert.Equal("Invalid or expired refresh token", ex.Message);
        }
    }
}