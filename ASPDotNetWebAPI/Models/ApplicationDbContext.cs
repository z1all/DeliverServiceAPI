using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DishBasket> DishBaskets { get; set; }
        public DbSet<DishInCart> DishInCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Hierarchy> Hierarchys { get; set; }
        public DbSet<AddressElement> AddressElements { get; set; }
        public DbSet<DeletedTokens> DeletedTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<DishBasket>()
                .HasOne(dishBasket => dishBasket.Dish)
                .WithMany(dish => dish.DishBaskets)
                .HasForeignKey(dishBasket => dishBasket.DishId)
                .IsRequired();
            modelBuilder.Entity<DishBasket>()
                .HasOne(dishBasket => dishBasket.User)
                .WithMany(user => user.DishBaskets)
                .HasForeignKey(dishBasket => dishBasket.UserId)
                .IsRequired();
            modelBuilder.Entity<DishBasket>()
                .HasOne(dishBasket => dishBasket.DishInCart)
                .WithMany(dishInCart => dishInCart.DishBaskets)
                .HasForeignKey(dishBasket => dishBasket.DishInCartId)
                .IsRequired();
            modelBuilder.Entity<DishBasket>()
                .HasKey(dishBasket => new { dishBasket.DishId, dishBasket.UserId, dishBasket.DishInCartId });

            modelBuilder.Entity<DishInCart>()
                .HasOne(dishBasket => dishBasket.User)
                .WithMany(user => user.DishInCarts)
                .HasForeignKey(dishBasket => dishBasket.UserId)
                .IsRequired();
            modelBuilder.Entity<DishInCart>()
                .HasOne(dishInCart => dishInCart.Order)
                .WithMany()
                .HasForeignKey(dishInCart => dishInCart.OrderId)
                .IsRequired(false);
            modelBuilder.Entity<DishInCart>()
                .HasKey(dishInCart => dishInCart.Id);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId)
                .IsRequired();
        }
    }
}
