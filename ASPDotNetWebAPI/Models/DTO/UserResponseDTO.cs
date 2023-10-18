using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Guid? AddressId { get; set; }
        [MaxLength(1)]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
