namespace ASPDotNetWebAPI.Models
{
    public class Rating
    {
        public Guid UserId { get; set; }
        public Guid DishId { get; set; }
        public double Value { get; set; }

        public User User { get; set; }
        public Dish Dish { get; set; }
    }
}
