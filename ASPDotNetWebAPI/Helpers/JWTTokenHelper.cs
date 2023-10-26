using ASPDotNetWebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPDotNetWebAPI.Helpers
{
    public static class JWTTokenHelper
    {
        public static string GeneratJWTToken(User user, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("JTI", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "HITs"
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public static string GetValueFromToken(HttpContext httpContext, string type)
        {
            var userGuidStr = httpContext.User.Claims.First(claim => claim.Type == type);

            return userGuidStr.Value;
        }

        public static Guid GetUserIdFromToken(HttpContext httpContext)
        {
            var userGuidStr = httpContext.User.Claims.First(claim => claim.Type == "UserId");

            return Guid.Parse(userGuidStr.Value);
        }

        public static Guid GetJTIFromToken(HttpContext httpContext)
        {
            var userGuidStr = httpContext.User.Claims.First(claim => claim.Type == "JTI");

            return Guid.Parse(userGuidStr.Value);
        }

        public static string GetTokenFromHeader(HttpContext httpContext)
        {
            return httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }
    }
}
