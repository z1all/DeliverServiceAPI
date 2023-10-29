using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsVegetairian { get; set; }
        public string Image { get; set; }
        public decimal? Rating { get; set; }
        public DishCategory Category { get; set; }

        // public ICollection<Rating> Ratings { get; set; }
    }
}
