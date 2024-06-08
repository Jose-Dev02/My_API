using MiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Repository
{
    public class WarehouseRepository : IRepository<Warehouse>
    {
        private readonly InventoryContext _inventoryContext;
        public WarehouseRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task AddAsync(Warehouse warehouse) => await _inventoryContext.Warehouse.AddAsync(warehouse);
        

        public void Delete(Warehouse warehouse)
        {
            _inventoryContext.Warehouse.Remove(warehouse);
        }

        public async Task<Warehouse> FindByIdAsync(int id) => await _inventoryContext.Warehouse.Include( b => b.Product )
                                                                                               .ThenInclude(b => b.Category )
                                                                                               .FirstOrDefaultAsync();

        public async Task<IEnumerable<Warehouse>> GetAllAsync() => await _inventoryContext.Warehouse.Include( b => b.Product )
                                                                                                    .ThenInclude( b => b.Category )
                                                                                                    .ToListAsync();

        public async Task Save() => await _inventoryContext.SaveChangesAsync();

        public IEnumerable<Warehouse> Search(Func<Warehouse, bool> filter) => _inventoryContext.Warehouse.Where(filter).ToList();

        public void Update(Warehouse warehouse)
        {
            _inventoryContext.Attach(warehouse);
            _inventoryContext.Entry(warehouse).State = EntityState.Modified;
        }
    }
}
