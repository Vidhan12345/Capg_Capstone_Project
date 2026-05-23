using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepo;
        private readonly IMapper _mapper;

        public DashboardService(IDashboardRepository dashboardRepo, IMapper mapper)
        {
            _dashboardRepo = dashboardRepo;
            _mapper = mapper;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var stats = await _dashboardRepo.GetStatsAsync();
            return _mapper.Map<DashboardStatsDto>(stats);
        }

        public async Task<IEnumerable<AttendanceTrendDto>> GetAttendanceTrendAsync(int days = 7)
        {
            var trend = await _dashboardRepo.GetAttendanceTrendAsync(days);
            return _mapper.Map<IEnumerable<AttendanceTrendDto>>(trend);
        }

        public async Task<IEnumerable<DepartmentDistributionDto>> GetDepartmentDistributionAsync()
        {
            var dist = await _dashboardRepo.GetDepartmentDistributionAsync();
            return _mapper.Map<IEnumerable<DepartmentDistributionDto>>(dist);
        }
    }
}
