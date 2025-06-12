using EcommercePlatform.Dtos;
using EcommercePlatform.Models;

namespace EcommercePlatform.Services
{
    public interface IReviewsService
    {
        Task<Guid> AddReviewAsync(Guid productId, AddReviewDto dto);
        public Task<IEnumerable<Reviews>> ReviewsOfProductAsync(Guid productId);
    }
}
