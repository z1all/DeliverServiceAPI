namespace ASPDotNetWebAPI.Models.DTO
{
    public class DishPagedListDTO
    {
        public List<DishDTO> Dishes { get; set; }
        public PageInfoDTO Pagination { get; set; }
    }
}
