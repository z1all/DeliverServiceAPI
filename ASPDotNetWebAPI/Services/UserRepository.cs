using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task Register(RegistrationRequestDTO model)
        {
            
        }
    }
}
