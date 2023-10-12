using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            //Database.EnsureCreated();
        }
    }
}
