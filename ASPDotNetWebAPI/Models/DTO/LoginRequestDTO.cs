using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [MinLength(1)]
        public string Email {  get; set; }
        [MinLength(1)]
        public string Password { get; set; }
    }
}
