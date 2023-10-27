using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task<TokenResponseDTO> RegisterAsync(RegistrationRequestDTO model);
        Task<TokenResponseDTO> LoginAsync(LoginRequestDTO model);
        Task LogoutAsync(string token);
        Task<UserResponseDTO> GetProfileAsync(string token);
        Task EditProfileAsync(string token, UserEditRequestDTO model);
    }
}
