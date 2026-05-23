using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<EmployeeDto>> GetPagedAsync(int page, int pageSize, string? search = null, string? sortBy = null, bool ascending = true, int? roleId = null)
        {
            var (items, total) = await _unitOfWork.Employees.GetPagedAsync(page, pageSize,
                filter: e =>
                    (string.IsNullOrEmpty(search) || (e.FirstName + " " + e.LastName).Contains(search) || e.Email.Contains(search) || e.EmployeeCode.Contains(search)) &&
                    (!roleId.HasValue || e.RoleId == roleId.Value),
                sortBy: sortBy ?? "EmployeeId", ascending: ascending);

            return new PagedResult<EmployeeDto>
            {
                Items = _mapper.Map<IEnumerable<EmployeeDto>>(items),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            if (await _unitOfWork.Employees.ExistsAsync(e => e.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            var employee = _mapper.Map<Employee>(dto);

            var lastCode = await _unitOfWork.Employees.FindAsync(e => e.EmployeeCode.StartsWith("EMP"));
            var maxNum = lastCode.Any()
                ? lastCode.Max(e => int.Parse(e.EmployeeCode.Replace("EMP", "")))
                : 0;
            employee.EmployeeCode = $"EMP{(maxNum + 1):D4}";
            employee.IsActive = true;

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.CompleteAsync();

            var username = dto.Email.Split('@')[0];
            if (await _unitOfWork.UserLogins.ExistsAsync(u => u.Username == username))
                username = $"{username}{employee.EmployeeId}";

            var userLogin = new UserLogin
            {
                EmployeeId = employee.EmployeeId,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                RoleId = dto.RoleId
            };
            await _unitOfWork.UserLogins.AddAsync(userLogin);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null) throw new KeyNotFoundException("Employee not found");

            if (dto.FirstName != null) employee.FirstName = dto.FirstName;
            if (dto.LastName != null) employee.LastName = dto.LastName;
            if (dto.PhoneNumber != null) employee.PhoneNumber = dto.PhoneNumber;
            if (dto.Gender != null) employee.Gender = dto.Gender;
            if (dto.DateOfJoining.HasValue) employee.DateOfJoining = dto.DateOfJoining.Value;
            if (dto.Address != null) employee.Address = dto.Address;
            if (dto.EmergencyContact != null) employee.EmergencyContact = dto.EmergencyContact;
            if (dto.DepartmentId.HasValue) employee.DepartmentId = dto.DepartmentId.Value;
            if (dto.RoleId.HasValue) employee.RoleId = dto.RoleId.Value;
            if (dto.IsActive.HasValue) employee.IsActive = dto.IsActive.Value;

            await _unitOfWork.Employees.UpdateAsync(employee);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null) throw new KeyNotFoundException("Employee not found");

            employee.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId)
        {
            var employees = await _unitOfWork.Employees.GetByDepartmentAsync(departmentId);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
    }
}
