namespace WMS.Application.DTOs
{
    public class AuditLogDto
    {
        public int AuditId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class AuditLogFilterDto
    {
        public string? EntityName { get; set; }
        public string? RecordId { get; set; }
        public string? Action { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}