using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;

namespace WMS_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AuditLogDto>>>> GetAll(
            [FromQuery] AuditLogFilterDto filter)
        {
            var result = await _auditLogService.GetPagedAsync(filter);
            return Ok(ApiResponse<PagedResult<AuditLogDto>>.Ok(result));
        }

        [HttpGet("entity/{entityName}/{recordId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuditLogDto>>>> GetByEntity(
            string entityName, string recordId)
        {
            var logs = await _auditLogService.GetByEntityAsync(entityName, recordId);
            return Ok(ApiResponse<IEnumerable<AuditLogDto>>.Ok(logs));
        }
    }
}