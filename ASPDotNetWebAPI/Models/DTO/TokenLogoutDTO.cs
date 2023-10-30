using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class TokenLogoutDTO
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
