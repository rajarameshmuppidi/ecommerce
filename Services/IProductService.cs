using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;

namespace EcommercePlatform.Services
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(string sellerId, Product product);
        Task<Product?> GetProductByIdAsync(Guid productId);

        Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto dto);

        Task<IEnumerable<Product>?> GetAllProductsAsync(ProductParameters parameters);

        Task<bool> DeleteByIdAsync(Guid id);
        
    }
}
