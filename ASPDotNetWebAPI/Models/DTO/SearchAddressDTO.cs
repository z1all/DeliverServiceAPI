using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class SearchAddressDTO
    {
        public int ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public string? Text { get; set; }
        public GarAddressLevel ObjectLevel {  get; set; }
        public string? ObjectLevelText { get; set; }
    }
}