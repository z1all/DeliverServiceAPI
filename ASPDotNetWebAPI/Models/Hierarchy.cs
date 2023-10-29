namespace ASPDotNetWebAPI.Models
{
    public class Hierarchy
    {
        public int Id { get; set; }
        public int Objectid { get; set; }
        public int Parentobjid { get; set; }
        public int Isactive { get; set; }
        public string Path { get; set; }
    }
}
