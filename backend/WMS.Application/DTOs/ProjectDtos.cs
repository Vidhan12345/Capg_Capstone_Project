namespace WMS.Application.DTOs
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public int ClientId { get; set; }
    }

    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; } = "NotStarted";
    }

    public class UpdateProjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClientId { get; set; }
        public string? Status { get; set; }
    }

    public class AllocationDto
    {
        public int AllocationId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? RoleOnProject { get; set; }
        public DateTime AllocatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateAllocationDto
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string? RoleOnProject { get; set; }
    }
}
