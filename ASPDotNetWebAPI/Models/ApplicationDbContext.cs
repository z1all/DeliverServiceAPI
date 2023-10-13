using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Dish> Dishes {  get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Rating>().HasNoKey();
           
            modelBuilder.Entity<Rating>()
                .HasOne(rating => rating.Dish)
                .WithMany(dish => dish.Ratings)
                .HasForeignKey(rating => rating.DishId)
                .IsRequired();
            modelBuilder.Entity<Rating>()
                .HasOne(rating => rating.User)
                .WithMany(user => user.Ratings)
                .HasForeignKey(rating => rating.UserId)
                .IsRequired();
            modelBuilder.Entity<Rating>()
                .HasKey(rating => new { rating.DishId, rating.UserId });

            //modelBuilder.Entity<Rating>().HasOne(x => x.User).WithMany(x => x.Ratings).IsRequired();

            //modelBuilder.Entity<Rating>().HasKey(x => new { x.User, x.Dish });
        }
    }
}
