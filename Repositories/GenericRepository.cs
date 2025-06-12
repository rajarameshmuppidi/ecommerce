
using EcommercePlatform.Data;
using EcommercePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(object Id)
        {
            return await _dbSet.FindAsync(Id);
        }
    }
}
