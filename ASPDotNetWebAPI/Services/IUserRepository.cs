using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task<TokenResponseDTO> Register(RegistrationRequestDTO model);
        Task<TokenResponseDTO> Login(LoginRequestDTO model);
    }
}
