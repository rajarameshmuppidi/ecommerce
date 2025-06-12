using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Services
{

    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository reviewsRepository;

        public ReviewsService(IReviewsRepository reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task<Guid> AddReviewAsync(Guid productId, AddReviewDto dto)
        {
            return await reviewsRepository.CreateReviewAsync(productId, dto);
        }

        public async Task<IEnumerable<Reviews>> ReviewsOfProductAsync(Guid productId)
        {
            var reviews = await reviewsRepository.ReviewsOfProductAsync(productId);
            return reviews;
        }
    }
}
