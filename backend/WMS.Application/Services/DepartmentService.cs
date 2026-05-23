using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            if (await _unitOfWork.Departments.ExistsAsync(d => d.Name == dto.Name))
                throw new InvalidOperationException("Department name already exists");

            var department = _mapper.Map<Department>(dto);
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null) throw new KeyNotFoundException("Department not found");

            if (dto.Name != null) department.Name = dto.Name;
            if (dto.Description != null) department.Description = dto.Description;

            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null) throw new KeyNotFoundException("Department not found");

            if (await _unitOfWork.Employees.ExistsAsync(e => e.DepartmentId == id))
                throw new InvalidOperationException("Cannot delete department with active employees");

            department.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
        }
    }
}
