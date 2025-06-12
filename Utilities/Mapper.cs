using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;

namespace EcommercePlatform.Utilities
{
    public class Mapper
    {
        private readonly AppDbContext dbcontext;

        public Mapper(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public ProductDto MapProductToDto(ProductDto product)
        {
            var reviews = dbcontext.Reviews.Where(r => r.Product.Id == product.Id).ToList();
            var seller = dbcontext.Sellers.Where(s => s.UserId == product.SellerId);
            return new ProductDto()
            {
                Id = product.Id,
                ProductTitle = product.ProductTitle,
                Reviews = reviews,
                Price = product.Price,
                SellerId = product.SellerId,
                ProductDescription = product.ProductDescription,
                Quantity = product.Quantity,
            };
        }
    }
}
