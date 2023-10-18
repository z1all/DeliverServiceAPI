using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task<TokenResponseDTO?> Register(RegistrationRequestDTO model);
        Task<TokenResponseDTO?> Login(LoginRequestDTO model);
        Task<bool> Logout(string token);
        Task<UserResponseDTO?> GetProfile (Guid userGuid);
        Task<bool> EditeProfile(Guid userGuid, UserEditRequestDTO model);
    }
}
