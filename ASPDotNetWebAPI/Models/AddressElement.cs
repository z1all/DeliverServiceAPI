namespace ASPDotNetWebAPI.Models
{
    public class AddressElement
    {
        public int Id { get; set; }
        public int Objectid { get; set; }
        public Guid Objectguid { get; set; }
        public string Name { get; set; }
        public string Typename { get; set; }
        public string Level { get; set; }
        public int Isactive { get; set; }
    }
}
