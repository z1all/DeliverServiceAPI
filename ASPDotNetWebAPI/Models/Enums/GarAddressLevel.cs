using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GarAddressLevel
    {
        [Description("Субъект РФ")] Region = 1,
        [Description("Административный район")] AdministrativeArea = 2,
        [Description("Муниципальный район")] MunicipalArea = 3,
        [Description("Сельское/городское поселение")] RuralUrbanSettlement = 4,
        [Description("Город")] City = 5,
        [Description("Населенный пункт")] Locality = 6, 
        [Description("Элемент планировочной структуры")] ElementOfPlanningStructure = 7, 
        [Description("Элемент улично-дорожной сети")] ElementOfRoadNetwork = 8, 
        [Description("Земельный участок")] Land = 9, 
        [Description("Здание (сооружение)")] Building = 10,
        [Description("Помещение")] Room = 11, 
        [Description("Помещения в пределах помещения")] RoomInRooms = 12, 
        [Description("Уровень автономного округа (устаревшее)")] AutonomousRegionLevel = 13,
        [Description("Уровень внутригородской территории (устаревшее)")] IntracityLevel = 14, 
        [Description("Уровень дополнительных территорий (устаревшее)")] AdditionalTerritoriesLevel = 15,
        [Description("Уровень объектов на дополнительных территориях (устаревшее)")] LevelOfObjectsInAdditionalTerritories = 16, 
        [Description("Машино-место")] CarPlace = 17
    }
}