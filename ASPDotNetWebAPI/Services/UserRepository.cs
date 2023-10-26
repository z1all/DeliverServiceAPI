using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private string? secretKey;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<TokenResponseDTO> RegisterAsync(RegistrationRequestDTO model)
        {
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
                Token = JWTTokenHelper.GeneratJWTToken(user, secretKey)
            };
        }

        public async Task<TokenResponseDTO?> LoginAsync(LoginRequestDTO model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.HashPassword))
            {
                return null;
            }

            return new TokenResponseDTO()
            {
                Token = JWTTokenHelper.GeneratJWTToken(user, secretKey)
            };
        }

        public async Task LogoutAsync(string token)
        {
            var JTI = JWTTokenHelper.GetValueFromToken(token, "UserId");

            await _dbContext.DeletedTokens.AddAsync(new() { TokenJTI = JTI });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserResponseDTO?> GetProfileAsync(string token)
        {
            Guid userGuid = Guid.Parse(JWTTokenHelper.GetValueFromToken(token, "UserId"));

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userGuid);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDTO()
            {
                Id = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                AddressId = user.AddressId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> EditProfileAsync(string token, UserEditRequestDTO model)
        {
            Guid userGuid = Guid.Parse(JWTTokenHelper.GetValueFromToken(token, "UserId"));

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userGuid);
            if (user == null)
            {
                return false;
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;
            user.AddressId = model.AddressId;
            user.PhoneNumber = model.PhoneNumber;

            return true;
        }

        public async Task<bool> EmailIsUsedAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            return user != null;
        }
    }
}
