using MiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly InventoryContext _inventoryContext;

        public UserRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task AddAsync(User entity) => await _inventoryContext.Users.AddAsync(entity);

        public void Delete(User entity)
        {
           _inventoryContext.Users.Remove(entity);
        }

        public async Task<User> FindByIdAsync(int id) => await _inventoryContext.Users.FindAsync(id);

        public async Task<User> Find(string user) => await _inventoryContext.Users.Where(b => b.Name.ToLower() == user.ToLower()).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetAllAsync() => await _inventoryContext.Users.ToListAsync();

        public async Task Save() => await _inventoryContext.SaveChangesAsync();

        public IEnumerable<User> Search(Func<User, bool> filter) => _inventoryContext.Users.Where(filter).ToList();

        public void Update(User entity)
        {
            _inventoryContext.Attach(entity);
            _inventoryContext.Entry(entity).State = EntityState.Modified; 
        }
    }
}
