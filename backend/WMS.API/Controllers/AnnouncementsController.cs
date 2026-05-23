using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;

namespace WMS_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AnnouncementDto>>>> GetAll()
        {
            var announcements = await _announcementService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<AnnouncementDto>>.Ok(announcements));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AnnouncementDto>>> GetById(int id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
                return NotFound(ApiResponse<AnnouncementDto>.Fail("Announcement not found"));
            return Ok(ApiResponse<AnnouncementDto>.Ok(announcement));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AnnouncementDto>>> Create([FromBody] CreateAnnouncementDto dto)
        {
            var postedBy = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var announcement = await _announcementService.CreateAsync(postedBy, dto);
            return Ok(ApiResponse<AnnouncementDto>.Ok(announcement, "Announcement created"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AnnouncementDto>>> Update(int id, [FromBody] UpdateAnnouncementDto dto)
        {
            var announcement = await _announcementService.UpdateAsync(id, dto);
            return Ok(ApiResponse<AnnouncementDto>.Ok(announcement, "Announcement updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            await _announcementService.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Announcement deleted"));
        }
    }
}
