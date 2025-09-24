using EcommercePlatform.Models;
using EcommercePlatform.Utilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace EcommercePlatform.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T?> GetByIdAsync(object Id);

        public Task<IEnumerable<T>> GetAllAsync(QueryParameters<T> parameters);

        public Task AddAsync(T entity);
        public void UpdateAsync(T entity);
        public void DeleteAsync(T entity);

    }
}