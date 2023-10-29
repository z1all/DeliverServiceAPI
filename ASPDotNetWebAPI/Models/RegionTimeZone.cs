namespace ASPDotNetWebAPI.Models
{
    public class RegionTimeZone
    {
        public Guid Id { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int TimeDifferenceWithMoscow { get; set; }
    }
}
