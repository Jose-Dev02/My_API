namespace MiApi.DTOs
{
    public class ProductInsertDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string? MediaURL { get; set; }
        public decimal Price { get; set; }
    }
}
