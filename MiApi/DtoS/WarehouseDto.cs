using MiApi.Models;

namespace MiApi.DTOs
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Cantidad { get; set; }
    }
}
