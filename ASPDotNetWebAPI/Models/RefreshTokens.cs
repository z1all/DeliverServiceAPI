using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models
{
    public class RefreshTokens
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }

        public User User { get; set; }
    }
}
