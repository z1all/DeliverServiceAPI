using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPDotNetWebAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private string secretKey;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        { 
            _dbContext = dbContext;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<TokenResponseDTO> Register(RegistrationRequestDTO model)
        {
            // Перенести в контроллер 
            var isUnique = !(await EmailIsUsed(model.Email));
            if (isUnique)
            {
                return new TokenResponseDTO()
                {
                    Token = null
                };
            }   

            var user = new User()
            {
                FullName = model.FullName,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, BCrypt.Net.BCrypt.GenerateSalt(12)),
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                AddressId = model.AddressId
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new TokenResponseDTO()
            {
                Token = GeneratJWTToken(user)
            };
        }

        public async Task<TokenResponseDTO> Login(LoginRequestDTO model)
        {
            // Перенести в контроллер 
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
            {
                return new TokenResponseDTO()
                {
                    Token = null
                };
            }

            if (BCrypt.Net.BCrypt.Verify(model.Password, user.HashPassword))
            {
                return new TokenResponseDTO()
                {
                    Token = null
                };
            }

            return new TokenResponseDTO()
            {
                Token = GeneratJWTToken(user)
            };
        }

        public async Task<bool> Logout(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if(!tokenHandler.CanReadToken(token))
            {
                return false;
            }

            var parsedToken = tokenHandler.ReadJwtToken(token);
            var JTI = parsedToken.Claims.FirstOrDefault(claim => claim.Type == "JTI");

            if (JTI == null)
            {
                return false;
            }

            await _dbContext.DeletedTokens.FirstOrDefaultAsync(token => token.TokenJTI == JTI.Value);

            return true;
        }

        private async Task<bool> EmailIsUsed(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            return user != null;
        }

        public string GeneratJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("JTI", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "HITs"
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
