using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class UserLogin : BaseEntity
    {
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
