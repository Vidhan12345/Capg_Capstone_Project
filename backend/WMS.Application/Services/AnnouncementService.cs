using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnnouncementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
        {
            await CleanupExpiredAsync();
            var announcements = await _unitOfWork.Announcements.FindAsync(a => a.IsActive);
            return _mapper.Map<IEnumerable<AnnouncementDto>>(announcements);
        }

        public async Task<AnnouncementDto?> GetByIdAsync(int id)
        {
            var announcement = await _unitOfWork.Announcements.GetByIdAsync(id);
            return announcement == null ? null : _mapper.Map<AnnouncementDto>(announcement);
        }

        public async Task<AnnouncementDto> CreateAsync(int postedBy, CreateAnnouncementDto dto)
        {
            await CleanupExpiredAsync();

            var announcement = new Announcement
            {
                Title = dto.Title,
                Content = dto.Content,
                PostedBy = postedBy,
                PostedAt = DateTime.UtcNow,
                ExpiresAt = dto.ExpiresAt,
                IsActive = true
            };

            await _unitOfWork.Announcements.AddAsync(announcement);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AnnouncementDto>(announcement);
        }

        public async Task<AnnouncementDto> UpdateAsync(int id, UpdateAnnouncementDto dto)
        {
            var announcement = await _unitOfWork.Announcements.GetByIdAsync(id);
            if (announcement == null) throw new KeyNotFoundException("Announcement not found");

            if (dto.Title != null) announcement.Title = dto.Title;
            if (dto.Content != null) announcement.Content = dto.Content;
            if (dto.ExpiresAt != null) announcement.ExpiresAt = dto.ExpiresAt;
            if (dto.IsActive.HasValue) announcement.IsActive = dto.IsActive.Value;

            await _unitOfWork.Announcements.UpdateAsync(announcement);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AnnouncementDto>(announcement);
        }

        public async Task DeleteAsync(int id)
        {
            var announcement = await _unitOfWork.Announcements.GetByIdAsync(id);
            if (announcement == null) throw new KeyNotFoundException("Announcement not found");

            await _unitOfWork.Announcements.DeleteAsync(announcement);
            await _unitOfWork.CompleteAsync();
        }

        private async Task CleanupExpiredAsync()
        {
            var now = DateTime.UtcNow;
            var expired = await _unitOfWork.Announcements.FindAsync(a => a.ExpiresAt != null && a.ExpiresAt <= now);
            foreach (var announcement in expired)
            {
                announcement.IsActive = false;
            }
            if (expired.Any())
            {
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
