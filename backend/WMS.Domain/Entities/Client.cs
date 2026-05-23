using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Client : BaseEntity
    {
        public int ClientId { get; set; }
        [Required, MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;
        public string? ClientAddress { get; set; }
        [MaxLength(20)]
        public string? ClientPhoneNumber { get; set; }
        [MaxLength(200)]
        public string? ClientLocation { get; set; }
        public bool Status { get; set; } = true;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
