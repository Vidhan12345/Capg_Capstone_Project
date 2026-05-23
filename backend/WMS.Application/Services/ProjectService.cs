using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProjectDto>> GetPagedAsync(int page, int pageSize, string? sortBy = null, bool ascending = true)
        {
            var (items, total) = await _unitOfWork.Projects.GetPagedAsync(page, pageSize, sortBy: sortBy ?? "ProjectId", ascending: ascending);
            return new PagedResult<ProjectDto>
            {
                Items = _mapper.Map<IEnumerable<ProjectDto>>(items),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            return project == null ? null : _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(dto.ClientId);
            if (client == null) throw new KeyNotFoundException("Client not found");

            var project = _mapper.Map<Project>(dto);
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null) throw new KeyNotFoundException("Project not found");

            if (dto.Name != null) project.Name = dto.Name;
            if (dto.Description != null) project.Description = dto.Description;
            if (dto.StartDate.HasValue) project.StartDate = dto.StartDate.Value;
            if (dto.EndDate != null) project.EndDate = dto.EndDate;
            if (dto.ClientId.HasValue) project.ClientId = dto.ClientId.Value;
            if (dto.Status != null) project.Status = dto.Status;

            await _unitOfWork.Projects.UpdateAsync(project);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null) throw new KeyNotFoundException("Project not found");

            project.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<AllocationDto>> GetAllocationsByProjectAsync(int projectId)
        {
            var allocations = await _unitOfWork.Projects.GetAllocationsByProjectAsync(projectId);
            return _mapper.Map<IEnumerable<AllocationDto>>(allocations);
        }

        public async Task<IEnumerable<AllocationDto>> GetMyAllocationsAsync(int employeeId)
        {
            var allocations = await _unitOfWork.Projects.GetAllocationsByEmployeeAsync(employeeId);
            return _mapper.Map<IEnumerable<AllocationDto>>(allocations.Where(a => a.IsActive));
        }

        public async Task<AllocationDto> AllocateEmployeeAsync(CreateAllocationDto dto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(dto.EmployeeId);
            if (employee == null || !employee.IsActive)
                throw new InvalidOperationException("Employee not found or inactive");

            var project = await _unitOfWork.Projects.GetByIdAsync(dto.ProjectId);
            if (project == null || project.Status == "Completed")
                throw new InvalidOperationException("Project not found or already completed");

            var alreadyAllocated = await _unitOfWork.Projects.IsEmployeeAllocatedAsync(dto.EmployeeId, dto.ProjectId);
            if (alreadyAllocated)
                throw new InvalidOperationException("Employee is already allocated to this project");

            var allocation = new EmployeeProjectAllocation
            {
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId,
                RoleOnProject = dto.RoleOnProject,
                IsActive = true
            };

            await _unitOfWork.Projects.AddAllocationAsync(allocation);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AllocationDto>(allocation);
        }

        public async Task ReleaseEmployeeAsync(int allocationId)
        {
            var allocation = await _unitOfWork.Projects.GetAllocationByIdAsync(allocationId);
            if (allocation == null) throw new KeyNotFoundException("Allocation not found");

            allocation.IsActive = false;
            allocation.ReleasedAt = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();
        }
    }
}
