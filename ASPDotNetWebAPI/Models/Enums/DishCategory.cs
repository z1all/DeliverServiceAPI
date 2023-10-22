using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DishCategory
    {
        Wok, Pizza, Soup, Desert, Drink
    }
}
