using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<AuditLogDto>> GetPagedAsync(AuditLogFilterDto filter)
        {
            var (items, total) = await _unitOfWork.AuditLogs.GetPagedAsync(
                filter.Page, filter.PageSize,
                filter: a =>
                    (string.IsNullOrEmpty(filter.EntityName) || a.EntityName == filter.EntityName) &&
                    (string.IsNullOrEmpty(filter.RecordId) || a.RecordId == filter.RecordId) &&
                    (string.IsNullOrEmpty(filter.Action) || a.Action == filter.Action) &&
                    (!filter.CreatedBy.HasValue || a.CreatedBy == filter.CreatedBy) &&
                    (!filter.FromDate.HasValue || a.CreatedOn >= filter.FromDate.Value) &&
                    (!filter.ToDate.HasValue || a.CreatedOn <= filter.ToDate.Value),
                sortBy: "CreatedOn", ascending: false);

            return new PagedResult<AuditLogDto>
            {
                Items = _mapper.Map<IEnumerable<AuditLogDto>>(items),
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string entityName, string recordId)
        {
            var logs = await _unitOfWork.AuditLogs.GetByEntityAsync(entityName, recordId);
            return _mapper.Map<IEnumerable<AuditLogDto>>(logs);
        }
    }
}