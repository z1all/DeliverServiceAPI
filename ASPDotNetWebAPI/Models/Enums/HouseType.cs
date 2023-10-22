using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HouseType
    {
        [Description("влд.")] ownership = 1,
        [Description("д.")] house = 2,
        [Description("двлд.")] household = 3,
        [Description("г-ж")] garage = 4,
        [Description("зд.")] building = 5,
        [Description("шахта")] mine = 6,
        [Description("стр.")] structure = 7,
        [Description("соор.")] facility = 8,
        [Description("литера")] letter = 9,
        [Description("к.")] corpus = 10,
        [Description("подв.")] basement = 11,
        [Description("кот.")] boilerRoom = 12,
        [Description("п-б")] cellar = 13,
        [Description("ОНС")] constructionInProgress = 14
    }
}