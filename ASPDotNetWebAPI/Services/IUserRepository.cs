using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task<TokenResponseDTO> RegisterAsync(RegistrationRequestDTO model);
        Task<TokenResponseDTO> LoginAsync(LoginRequestDTO model);
        Task LogoutAllAsync(Guid JTI);
        Task LogoutCurrentAsync(Guid JTI, TokenLogoutDTO refreshToken);
        Task<TokenResponseDTO> Refresh(RefreshDTO refreshDTO);
        Task<UserResponseDTO> GetProfileAsync(Guid userId);
        Task EditProfileAsync(Guid userId, UserEditRequestDTO model);
    }
}
