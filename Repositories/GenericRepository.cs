
using EcommercePlatform.Data;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(QueryParameters<T> paramters)
        {
            IQueryable<T> query = _dbSet;

            if(paramters.Filter != null) query = query.Where(paramters.Filter);

            if(paramters.MinMaxCondition != null) query = query.Where(paramters.MinMaxCondition);

            if(paramters.Includes != null)
            {
                foreach(var include in paramters.Includes)
                {
                    query = query.Include(include);
                }
            }

            if(paramters.OrderBy != null)
            {
                query = paramters.OrderBy(query);
            }

            if(paramters.Page.HasValue && paramters.PageSize.HasValue)
            {
                int skip = (paramters.Page.Value - 1) * paramters.PageSize.Value;
                query = query.Skip(skip).Take(paramters.PageSize.Value);
            }

            return await query.ToListAsync();

        }

        public async Task<T?> GetByIdAsync(object Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}

    