using System.Linq.Expressions;
using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using Xunit;

namespace WMS.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserLoginRepository> _userLoginRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userLoginRepoMock = new Mock<IUserLoginRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock.Setup(u => u.UserLogins).Returns(_userLoginRepoMock.Object);
            _service = new EmployeeService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
        {
            var dto = new CreateEmployeeDto { Email = "duplicate@test.com", FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(2000, 1, 1), DepartmentId = 1, RoleId = 1 };
            _unitOfWorkMock.Setup(u => u.Employees.ExistsAsync(e => e.Email == dto.Email)).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
            Assert.Equal("Email already exists", ex.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsEmployeeDto()
        {
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe" };
            var expectedDto = new EmployeeDto { EmployeeId = 1, FirstName = "John", LastName = "Doe" };
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(1)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(expectedDto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.EmployeeId);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(99)).ReturnsAsync((Employee?)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_WithValidData_CreatesEmployee()
        {
            var dto = new CreateEmployeeDto { Email = "new@test.com", FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(2000, 1, 1), DepartmentId = 1, RoleId = 1 };
            var employee = new Employee { EmployeeId = 1, Email = "new@test.com" };
            var expectedDto = new EmployeeDto { EmployeeId = 1, Email = "new@test.com" };

            _unitOfWorkMock.Setup(u => u.Employees.ExistsAsync(e => e.Email == dto.Email)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Employees.FindAsync(e => e.EmployeeCode.StartsWith("EMP"))).ReturnsAsync(new List<Employee>());
            _userLoginRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<UserLogin, bool>>>())).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Employee>(dto)).Returns(employee);
            _unitOfWorkMock.Setup(u => u.Employees.AddAsync(employee)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(expectedDto);

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("new@test.com", result.Email);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(99)).ReturnsAsync((Employee?)null);

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
            Assert.Equal("Employee not found", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_SoftDeletesEmployee()
        {
            var employee = new Employee { EmployeeId = 1, IsDeleted = false };
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(1)).ReturnsAsync(employee);

            await _service.DeleteAsync(1);

            Assert.True(employee.IsDeleted);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_GeneratesEmployeeCode()
        {
            var dto = new CreateEmployeeDto { Email = "code@test.com", FirstName = "Test", LastName = "User", DateOfBirth = new DateTime(2000, 1, 1), DepartmentId = 1, RoleId = 1 };
            var existing = new List<Employee> { new Employee { EmployeeCode = "EMP0001" }, new Employee { EmployeeCode = "EMP0002" } };
            var employee = new Employee();
            var expectedDto = new EmployeeDto { EmployeeCode = "EMP0003" };

            _unitOfWorkMock.Setup(u => u.Employees.ExistsAsync(e => e.Email == dto.Email)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Employees.FindAsync(e => e.EmployeeCode.StartsWith("EMP"))).ReturnsAsync(existing);
            _userLoginRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<UserLogin, bool>>>())).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Employee>(dto)).Returns(employee);
            _unitOfWorkMock.Setup(u => u.Employees.AddAsync(It.IsAny<Employee>())).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(expectedDto);

            var result = await _service.CreateAsync(dto);

            Assert.Equal("EMP0003", result.EmployeeCode);
        }
    }
}