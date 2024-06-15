using Microsoft.EntityFrameworkCore;

namespace MiApi.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext (DbContextOptions<InventoryContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
