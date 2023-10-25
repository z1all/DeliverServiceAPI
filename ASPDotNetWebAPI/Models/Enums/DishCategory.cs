using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DishCategory
    {
        Wok = 1, 
        Pizza = 2, 
        Soup = 3, 
        Desert = 4, 
        Drink = 5
    }
}
