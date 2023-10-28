namespace ASPDotNetWebAPI.Models
{
    public class House
    {
        public int Id { get; set; }
        public int Objectid { get; set; }
        public Guid Objectguid { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Housenum { get; set; }
        public string? Addnum1 { get; set; }
        public string? Addnum2 { get; set; }
        public int? Housetype { get; set; }
        public int? Addtype1 { get; set; }
        public int? Addtype2 { get; set; }
        public int Isactive { get; set; }
    }
}
