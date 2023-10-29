using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class DishBasketDTO
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(1)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int Amount { get; set; }
        public string Image { get; set; }
    }
}
