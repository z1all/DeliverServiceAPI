using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class UserEditRequestDTO
    {
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Guid? AddressId { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
