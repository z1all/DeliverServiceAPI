using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class TokenResponseDTO
    {
        [Required]
        public string Token { get; set; }
    }
}
