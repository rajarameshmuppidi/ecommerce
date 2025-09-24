using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Repositories;
using EcommercePlatform.Utilities;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{

    public class ProductService(IProductsRepository productsRepository) : IProductService
    {
        public async Task<string> CreateProductAsync(string sellerId, Product product)
        {
            var res = await productsRepository.CreateProductAsync(sellerId, product);
            return res.ToString();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var res = await productsRepository.DeleteProductAsync(id);
            return res;
        }

        public async Task<IEnumerable<Product>?> GetAllProductsAsync(ProductParameters parameters)
        {
            var res = await productsRepository.GetProductsAsync(parameters);
            return res;
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            var product = await productsRepository.GetProductDetailsByIdAsync(productId);
            
            
            if (product == null) return null;

            var productDto = new ProductDto
            {
                Id = product.Id,
                ProductDescription = product.ProductDescription,
                ProductTitle = product.ProductTitle,
                Price = product.Price,
                Quantity = product.Quantity,
                SellerId = product.SellerId,
                Seller = product.Seller,
                Reviews = product.Reviews?.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    ReviewText = r.Review,
                    UserName = r?.User?.AppUser?.UserName,
                    CreatedAt = r.CreatedAt
                }).ToList() ?? new List<ReviewDto>()
            };

            return productDto;
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto dto)
        {
            var res = await productsRepository.UpdateProductAsync(id,dto);
            return res;
        }

        public async Task<List<(Guid ProductId, string ProductTitle, int PendingOrderedQuantity)>> GetPendingOrderQuantitiesForSellerAsync(string sellerId)
        {
            return await productsRepository.GetPendingOrderQuantitiesForSellerAsync(sellerId);
        }

        public async Task<List<(Guid ProductId, int PendingOrderedQuantity)>> GetPendingOrderCountsForSellerAsync(
            string sellerId,
            float minPrice,
            float maxPrice,
            int pageSize,
            int pageNumber,
            string? searchString,
            DateTime? fromDate,
            DateTime? toDate)
        {
            return await productsRepository.GetPendingOrderCountsForSellerAsync(
                sellerId, minPrice, maxPrice, pageSize, pageNumber, searchString, fromDate, toDate);
        }
    }
}
