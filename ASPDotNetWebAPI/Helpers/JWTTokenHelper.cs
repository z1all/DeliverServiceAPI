using ASPDotNetWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ASPDotNetWebAPI.Helpers
{
    public static class JWTTokenHelper
    {
        public static string GeneratJWTToken(User user, IConfiguration configuration)
        {
            var secret = configuration["JWTTokenSettings:Secret"] ?? throw new InvalidOperationException("Secret not configured");
            var ValidIssuer = configuration["JWTTokenSettings:ValidIssuer"] ?? throw new InvalidOperationException("ValidIssuer not configured");
            var AccessTokenLifeTimeMinut = configuration["JWTTokenSettings:AccessTokenLifeTimeMinut"]
                ?? throw new InvalidOperationException("AccessTokenLifeTimeMinut not configured");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("JTI", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(AccessTokenLifeTimeMinut)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = ValidIssuer
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public static string? GetValueFromToken(string token, string type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(token);

            var userGuidStr = parsedToken.Claims.First(claim => claim.Type == type);

            return (userGuidStr != null) ? userGuidStr.Value : null;
        }

        public static string? GetValueFromToken(HttpContext httpContext, string type)
        {
            var userGuidStr = httpContext.User.Claims.First(claim => claim.Type == type);

            return (userGuidStr != null) ? userGuidStr.Value : null;
        }

        public static Guid GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(token);

            var userGuidStr = parsedToken.Claims.First(claim => claim.Type == "UserId");

            return Guid.Parse(userGuidStr.Value);
        }

        public static Guid GetUserIdFromToken(HttpContext httpContext)
        {
            var userGuidStr = httpContext.User.Claims.First(claim => claim.Type == "UserId");

            return Guid.Parse(userGuidStr.Value);
        }

        public static Guid GetJTIFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(token);

            var userGuidStr = parsedToken.Claims.First(claim => claim.Type == "JTI");

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

        public static bool ValidateToken(string token, IConfiguration configuration)
        {
            var secret = configuration["JWTTokenSettings:Secret"] ?? throw new InvalidOperationException("Secret not configured");
              
            var validation = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateIssuer = true,
                ValidIssuer = configuration["JWTTokenSettings:ValidIssuer"],
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken? validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validation, out validatedToken);
            }
            catch (SecurityTokenException) { return false; }
            catch (ArgumentNullException) { return false; }
            catch (ArgumentException) { return false; }
            catch (Exception)
            {
                throw;
            }
            
            return validatedToken != null;
        }
    }
}
