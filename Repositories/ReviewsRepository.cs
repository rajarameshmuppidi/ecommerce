using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace EcommercePlatform.Repositories
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly AppDbContext dbContext;

        public ReviewsRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> CreateReviewAsync(Guid productId, AddReviewDto dto)
        {
            var ReviewToBeCreated = new Reviews()
            {
                ProductId = productId,
                Rating = dto.Rating,
                UserId = dto.UserId,
                Review = dto.ReviewString,
            };

            await dbContext.Reviews.AddAsync(ReviewToBeCreated);

            int res = await dbContext.SaveChangesAsync();

            if (res > 0) return ReviewToBeCreated.Id ; return Guid.Empty;

        }

        public async Task<IEnumerable<Reviews>> ReviewsOfProductAsync(Guid productId)
        {
            var Reviews = await dbContext.Reviews.ToListAsync();
            return Reviews;
        }

    }
}
