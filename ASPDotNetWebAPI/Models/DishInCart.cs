namespace ASPDotNetWebAPI.Models
{
    public class DishInCart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Count { get; set; }

        public User User { get; set; }
        public ICollection<DishBasket> DishBaskets { get; set; }
    }
}
