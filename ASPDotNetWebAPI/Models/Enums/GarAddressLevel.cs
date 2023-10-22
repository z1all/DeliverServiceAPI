using System.Text.Json.Serialization;

namespace ASPDotNetWebAPI.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GarAddressLevel
    {
        Region, AdministrativeArea, MunicipalArea, RuralUrbanSettlement, City,
        Locality, ElementOfPlanningStructure, ElementOfRoadNetwork, Land, Building,
        Room, RoomInRooms, AutonomousRegionLevel, IntracityLevel, AdditionalTerritoriesLevel,
        LevelOfObjectsInAdditionalTerritories, CarPlace
    }
}