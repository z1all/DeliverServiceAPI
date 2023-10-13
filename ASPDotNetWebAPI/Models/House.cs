namespace ASPDotNetWebAPI.Models
{
    public class House
    {
        public int Id { get; set; }
        public int Objectid { get; set; }
        public Guid Objectguid { get; set; }
        public string? Housenum { get; set; }
        public int Isactive { get; set; }
    }
}
