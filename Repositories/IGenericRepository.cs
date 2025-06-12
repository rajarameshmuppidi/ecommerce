using EcommercePlatform.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace EcommercePlatform.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(object Id);
    }
}