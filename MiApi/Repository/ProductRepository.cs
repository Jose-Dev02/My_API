
using MiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly InventoryContext _inventoryContext;

        public ProductRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }
        public async Task AddAsync(Product entity) => await _inventoryContext.Products.AddAsync(entity);


        public void Delete(Product entity)
        {
            _inventoryContext.Products.Remove(entity);
        }
        public async Task<Product> FindByIdAsync(int id) => await _inventoryContext.Products.Where(b => b.Id == id)
                                                                                            .Include(b => b.Category)
                                                                                            .FirstOrDefaultAsync();


        public async Task<IEnumerable<Product>> GetAllAsync() => await _inventoryContext.Products.Include( b => b.Category)
                                                                                                 .ToListAsync();

        public async Task Save() => await _inventoryContext.SaveChangesAsync();

        public IEnumerable<Product> Search(Func<Product, bool> filter) => _inventoryContext.Products.Where(filter).ToList();

        public void Update(Product entity)
        {
            _inventoryContext.Attach(entity);
            _inventoryContext.Entry(entity).State = EntityState.Modified;
        }
        
    }
}
