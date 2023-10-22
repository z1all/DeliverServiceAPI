using ASPDotNetWebAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class UserResponseDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public Guid? AddressId { get; set; }
        [Required]
        [MaxLength(1)]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
