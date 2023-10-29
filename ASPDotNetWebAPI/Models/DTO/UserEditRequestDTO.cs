using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class UserEditRequestDTO
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public Guid? AddressId { get; set; }
        [CustomPhone(Nullable = true)]
        public string? PhoneNumber { get; set; }
    }
}
