using MiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly InventoryContext _inventoryContext;

        public CategoryRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task AddAsync(Category category) =>  await _inventoryContext.Categories.AddAsync(category);
        

        public void Delete(Category category)
        {
            _inventoryContext.Categories.Remove(category);
        }

        public async Task<Category> FindByIdAsync(int id) => await _inventoryContext.Categories.FindAsync(id);

        public async Task<IEnumerable<Category>> GetAllAsync() => await _inventoryContext.Categories.ToListAsync();

        public async Task Save() => await _inventoryContext.SaveChangesAsync();

        public  IEnumerable<Category> Search(Func<Category, bool> filter) => _inventoryContext.Categories.Where(filter).ToList();

        public void Update(Category category)
        {
            _inventoryContext.Attach(category);
            _inventoryContext.Entry(category).State = EntityState.Modified; 
        }
    }
}
