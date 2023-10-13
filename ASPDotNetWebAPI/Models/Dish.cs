namespace ASPDotNetWebAPI.Models
{
    public enum DishCategory
    {
        Wok, Pizza, Soup, Desert, Drink
    }

    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public bool IsVegetairian { get; set; }
        public string Image {  get; set; }
        public DishCategory Category { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
