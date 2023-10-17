using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [MinLength(1)]
        public string email {  get; set; }
        [MinLength(1)]
        public string password { get; set; }
    }
}
