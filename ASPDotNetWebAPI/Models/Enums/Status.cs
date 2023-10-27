using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        InProcess = 1, 
        Delivered = 2
    }
}
