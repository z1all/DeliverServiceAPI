namespace ASPDotNetWebAPI.Models
{
    public enum Gender
    {
        Male, Female
    }

    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid AddressId { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<DishBasket> DishBaskets { get; set; }
        public ICollection<DishInCart> DishInCarts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
