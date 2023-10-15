using ASPDotNetWebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Services
{
    public interface IUserRepository
    {
        Task Register(RegistrationRequestDTO model);
    }
}
