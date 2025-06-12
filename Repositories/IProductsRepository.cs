using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;
using System.Security;

namespace EcommercePlatform.Repositories
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProductsAsync(ProductParameters parameters);
        Task<Product?> GetProductDetailsByIdAsync(Guid id);

        Task<Guid> CreateProductAsync(string sellerId, Product product);

        Task<bool> UpdateProductAsync(Guid productId, ProductUpdateDto latestProduct);

        Task<bool> DeleteProductAsync(Guid productId);
    }
}
