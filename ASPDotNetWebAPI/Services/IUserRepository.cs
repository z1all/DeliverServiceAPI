using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task<TokenResponseDTO> RegisterAsync(RegistrationRequestDTO model);
        Task<TokenResponseDTO> LoginAsync(LoginRequestDTO model);
        Task LogoutAsync(Guid JTI);
        Task<UserResponseDTO> GetProfileAsync(Guid UserId);
        Task EditProfileAsync(Guid UserId, UserEditRequestDTO model);
    }
}
