﻿using ASPDotNetWebAPI.Exceptions;
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
            var isNotUnique = await EmailIsUsedAsync(model.Email);

            if (isNotUnique)
            {
                throw new ValidationProblemException($"Username '{model.Email}' is already taken.");
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
                Token = JWTTokenHelper.GeneratJWTToken(user, secretKey)
            };
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

            return new TokenResponseDTO()
            {
                Token = JWTTokenHelper.GeneratJWTToken(user, secretKey)
            };
        }

        public async Task LogoutAsync(Guid JTI)
        {
            await _dbContext.DeletedTokens.AddAsync(new() { TokenJTI = JTI.ToString() });
            await _dbContext.SaveChangesAsync();
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