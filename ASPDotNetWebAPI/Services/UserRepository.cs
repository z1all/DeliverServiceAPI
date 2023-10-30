using ASPDotNetWebAPI.Exceptions;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private int saltNum;
        private int refreshTokenTimeLifeDay;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            refreshTokenTimeLifeDay = configuration.GetValue<int>("JWTTokenSettings:RefreshTokenLifeTimeDay");
            saltNum = configuration.GetValue<int>("PasswordHashSettings:SaltNum");
        }

        public async Task<TokenResponseDTO> RegisterAsync(RegistrationRequestDTO model)
        {
            var isNotUnique = await EmailIsUsedAsync(model.Email);

            if (isNotUnique)
            {
                throw new ValidationProblemException($"Username '{model.Email}' is already taken.");
            }

            var user = new User()
            {
                FullName = model.FullName,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, BCrypt.Net.BCrypt.GenerateSalt(saltNum)),
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                AddressId = model.AddressId
            };

            await _dbContext.Users.AddAsync(user);
            var tokens = await GetTokensAndAddToDb(user);

            await _dbContext.SaveChangesAsync();

            return tokens;
        }

        public async Task<TokenResponseDTO> LoginAsync(LoginRequestDTO model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
            {
                throw new NotFoundException("Login failed. A user with this username and password was not found!");
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.HashPassword))
            {
                throw new NotFoundException("Login failed. A user with this username and password was not found!");
            }

            var tokens = await GetTokensAndAddToDb(user);
            await _dbContext.SaveChangesAsync();

            return tokens;
        }

        private async Task<TokenResponseDTO> GetTokensAndAddToDb (User user)
        {
            var tokens = new TokenResponseDTO()
            {
                AccessToken = JWTTokenHelper.GeneratJWTToken(user, _configuration),
                RefreshToken = JWTTokenHelper.GenerateRefreshToken()
            };
            await _dbContext.RefreshTokens.AddAsync(new RefreshTokens()
            {
                RefreshToken = tokens.RefreshToken,
                AccessTokenJTI = JWTTokenHelper.GetJTIFromToken(tokens.AccessToken),
                User = user,
                Expires = DateTime.UtcNow.AddDays(refreshTokenTimeLifeDay)
            });

            return tokens;
        }

        public async Task LogoutAllAsync(Guid userId)
        {
            var usersRefreshTokens = await _dbContext.RefreshTokens.Where(refreshToken => refreshToken.UserId == userId).ToListAsync();
            _dbContext.RefreshTokens.RemoveRange(usersRefreshTokens);

            await _dbContext.SaveChangesAsync();
        }

        public async Task LogoutCurrentAsync(Guid userId, TokenLogoutDTO refreshToken)
        {
            var usersRefreshTokens = await _dbContext.RefreshTokens.FirstOrDefaultAsync(token => token.RefreshToken == refreshToken.RefreshToken);
            if (usersRefreshTokens == null)
            {
                throw new NotFoundException("Refresh token not found!");
            }

            _dbContext.RefreshTokens.Remove(usersRefreshTokens);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<TokenResponseDTO> RefreshAsync(RefreshDTO refreshDTO)
        {
            if (!JWTTokenHelper.ValidateToken(refreshDTO.AccessToken, _configuration))
            {
                throw new UnauthorizedException("Access token failed validation!");
            }

            var userId = JWTTokenHelper.GetUserIdFromToken(refreshDTO.AccessToken);

            var tokensFromDb = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(refreshTokens => refreshTokens.RefreshToken == refreshDTO.RefreshToken);
            if (tokensFromDb == null || tokensFromDb.UserId != userId || tokensFromDb.Expires < DateTime.UtcNow)
            {
                throw new UnauthorizedException("The Refresh token was not found, or is not associated with the Access token, or its lifetime has expired!");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            var tokens = new TokenResponseDTO()
            {
                RefreshToken = refreshDTO.RefreshToken,
                AccessToken = JWTTokenHelper.GeneratJWTToken(user, _configuration)
            };

            tokensFromDb.AccessTokenJTI = JWTTokenHelper.GetJTIFromToken(tokens.AccessToken);
            await _dbContext.SaveChangesAsync();

            return tokens;
        }

        public async Task<UserResponseDTO> GetProfileAsync(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
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

        public async Task EditProfileAsync(Guid userId, UserEditRequestDTO model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            if (user.Email != model.Email)
            {
                var userSameEmail = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == model.Email);
                if (userSameEmail != null)
                {
                    throw new ValidationProblemException($"A user with the same email {model.Email} already exists");
                }
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;
            user.AddressId = model.AddressId;
            user.PhoneNumber = model.PhoneNumber;

            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> EmailIsUsedAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            return user != null;
        }
    }
}