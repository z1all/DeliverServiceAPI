using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DishSorting
    {
        NameAsc, NameDesc, PriceAsc, PriceDesc, RatingAsc, RatingDesc
    }
}
