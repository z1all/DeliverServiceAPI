using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class SearchAddressDTO
    {
        public int ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public string Text { get; set; }
        public GarAddressLevel ObjectLevel { get; set; }
        public string? ObjectLevelText { get; set; }

        public SearchAddressDTO() { }
        public SearchAddressDTO(House house)
        {
            ObjectId = house.Objectid;
            ObjectGuid = house.Objectguid;
            Text = house.FullName;
            ObjectLevel = GarAddressLevel.Building;
            ObjectLevelText = GarAddressLevel.Building.GetDescription();
        }
        public SearchAddressDTO(AddressElement addressElement)
        {
            ObjectId = addressElement.Objectid;
            ObjectGuid = addressElement.Objectguid;
            Text = addressElement.Typename + " " + addressElement.Name;
            ObjectLevel = (GarAddressLevel)int.Parse(addressElement.Level);
            ObjectLevelText = ((GarAddressLevel)int.Parse(addressElement.Level)).GetDescription();
        }
    }
}