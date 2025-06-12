using EcommercePlatform.Dtos;
using EcommercePlatform.Models;

namespace EcommercePlatform.Repositories
{
    public interface IReviewsRepository
    {
        Task<IEnumerable<Reviews>> ReviewsOfProductAsync(Guid productId);

        Task<Guid> CreateReviewAsync(Guid productId, AddReviewDto dto);
    }
}
