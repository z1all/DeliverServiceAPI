using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [MinLength(1)]
        public string Email {  get; set; }
        [Required]
        [MinLength(1)]
        public string Password { get; set; }
    }
}
