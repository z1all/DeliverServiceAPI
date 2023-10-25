using ASPDotNetWebAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class DishDTO
    {
        public Guid Id { get; set; }
        [MinLength(1)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public string? Image { get; set; }
        public bool IsVegetairian { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Rating { get; set; }
        public DishCategory Category { get; set; }
    }
}
