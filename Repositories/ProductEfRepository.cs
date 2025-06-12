using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EcommercePlatform.Repositories
{
    public class ProductEfRepository : IProductsRepository
    {
        private readonly AppDbContext dbContext ;

        public ProductEfRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> CreateProductAsync(string sellerId, Product product)
        {
            try
            {
                product.SellerId = sellerId;
                await dbContext.Products.AddAsync(product);
                var creationStatus = await dbContext.SaveChangesAsync();
                if (creationStatus > 0)
                {
                    return product.Id;
                }
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return Guid.Empty;
            }
            catch (DbUpdateException ex)
            {
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
            return Guid.NewGuid();
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var FoundProduct = await dbContext.Products.FindAsync(productId);
            try
            {
                if (FoundProduct != null)
                {
                    dbContext.Products.Remove(FoundProduct);
                    int res = await dbContext.SaveChangesAsync();
                    if (res > 0) { return true; }
                }
                else
                {
                    return false;
                }
            }
            catch(DBConcurrencyException ex)
            {
                return false;
            }
            catch (Exception ex) {
                return false;
            }
            return false ;
        }

        public async Task<Product?> GetProductDetailsByIdAsync(Guid id)
        {
            var FoundProduct = await dbContext.Products.Include(p => p.Seller).FirstOrDefaultAsync(p => p.Id == id);
            
            if (FoundProduct != null)
            {
                var reviewsOfProduct = await dbContext.Reviews.Where(r => r.ProductId == id).ToListAsync();
                FoundProduct.Reviews = new List<Reviews>();
                FoundProduct.Reviews.AddRange(reviewsOfProduct);
            }

            return FoundProduct;
        }

        public async Task<List<Product>> GetProductsAsync(ProductParameters parameters)
        {

            IQueryable<Product> ProductsQuery = dbContext.Products.Where(p => p.Price >= parameters.minPrice && p.Price <= parameters.maxPrice).AsQueryable();

            if(parameters.sellerId != null)
            {
                ProductsQuery = ProductsQuery.Where(p => p.SellerId == parameters.sellerId);
            }

            switch (parameters.sortBy)
            {
                case SortSchemes.Popularity:
                    ProductsQuery = ProductsQuery.OrderBy(p => p.Price);
                    break;

                case SortSchemes.Price:
                    ProductsQuery = ProductsQuery.OrderBy(p => p.Price);
                    break;

                case SortSchemes.Relevance:
                    ProductsQuery = ProductsQuery.OrderBy(p => p.Price);
                    break;

                default:
                    break;
            }

            if (parameters.reverse) ProductsQuery = ProductsQuery.OrderByDescending(p => p.Price);

            if (!string.IsNullOrEmpty(parameters.searchString))
            {
                ProductsQuery = ProductsQuery.Where(p => p.ProductTitle.Contains(parameters.searchString)).Skip(parameters.pageSize * parameters.pageNumber).Take(parameters.pageSize);
            }
            else
            {
                ProductsQuery = ProductsQuery.Skip(parameters.pageSize * parameters.pageNumber).Take(parameters.pageSize);
            }

            return await ProductsQuery.ToListAsync();

        }

        public async Task<bool> UpdateProductAsync(Guid productId, ProductUpdateDto latestProduct)
        {
            try
            {
                var FoundProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (FoundProduct != null)
                {
                    if (latestProduct != null)
                    {
                        if (latestProduct.ProductTitle != null) FoundProduct.ProductTitle = latestProduct.ProductTitle;
                        if (latestProduct.ProductDescription != null) FoundProduct.ProductDescription = latestProduct.ProductDescription;
                        if (latestProduct.Price != null) FoundProduct.Price = latestProduct.Price ?? FoundProduct.Price;
                        if (latestProduct.Quantity != null) FoundProduct.Quantity = latestProduct.Quantity ?? FoundProduct.Quantity;
                    }
                    int saveChanges = await dbContext.SaveChangesAsync();

                    if (saveChanges > 0) { return true; }
                }
            }
            catch(DBConcurrencyException ex)
            {
                return false;
            }
            catch(DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            throw new NotImplementedException();
        }


    }
}
