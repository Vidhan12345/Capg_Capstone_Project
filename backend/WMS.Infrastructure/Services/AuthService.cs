using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.UserLogins.GetByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            var employee = await _unitOfWork.Employees.GetByIdAsync(user.EmployeeId);
            if (employee == null || !employee.IsActive)
                throw new UnauthorizedAccessException("Employee account is inactive");

            var (token, expiresAt) = GenerateJwtToken(user.Username, employee.EmployeeId, employee.Email, employee.Role?.Name ?? "Employee", employee.EmployeeCode);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("Jwt:RefreshTokenExpiryInDays", 7));
            user.LastLogin = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();

            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                Role = employee.Role?.Name,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                EmployeeId = employee.EmployeeId
            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = (await _unitOfWork.UserLogins.FindAsync(u => u.RefreshToken == request.RefreshToken)).FirstOrDefault();
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var employee = await _unitOfWork.Employees.GetByIdAsync(user.EmployeeId);
            if (employee == null || !employee.IsActive)
                throw new UnauthorizedAccessException("Employee account is inactive");

            var (token, expiresAt) = GenerateJwtToken(user.Username, employee.EmployeeId, employee.Email, employee.Role?.Name ?? "Employee", employee.EmployeeCode);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("Jwt:RefreshTokenExpiryInDays", 7));
            await _unitOfWork.CompleteAsync();

            return new LoginResponse
            {
                Token = token,
                RefreshToken = newRefreshToken,
                ExpiresAt = expiresAt,
                Role = employee.Role?.Name,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                EmployeeId = employee.EmployeeId
            };
        }

        public async Task LogoutAsync(int employeeId)
        {
            var user = await _unitOfWork.UserLogins.GetByEmployeeIdAsync(employeeId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task ChangePasswordAsync(int employeeId, string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.UserLogins.GetByEmployeeIdAsync(employeeId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _unitOfWork.CompleteAsync();
        }

        private (string token, DateTime expiresAt) GenerateJwtToken(string username, int employeeId, string email, string role, string employeeCode)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employeeId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("EmployeeCode", employeeCode)
            };

            var expiryMinutes = _configuration.GetValue<int>("Jwt:ExpiryInMinutes", 60);
            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
