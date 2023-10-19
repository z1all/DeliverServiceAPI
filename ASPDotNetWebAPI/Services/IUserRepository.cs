using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task<TokenResponseDTO> RegisterAsync(RegistrationRequestDTO model);
        Task<TokenResponseDTO?> LoginAsync(LoginRequestDTO model);
        Task<bool> LogoutAsync(string token);
        Task<UserResponseDTO?> GetProfileAsync(Guid userGuid);
        Task<bool> EditeProfileAsync(Guid userGuid, UserEditRequestDTO model);
        Task<bool> EmailIsUsedAsync(string email);
    }
}
