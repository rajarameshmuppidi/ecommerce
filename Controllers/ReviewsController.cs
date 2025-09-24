using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EcommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsService reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this.reviewsService = reviewsService;
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddReviewOfTheProduct([FromRoute] Guid productId, [FromBody] AddReviewDto dto)
        {
            var res = await reviewsService.AddReviewAsync(productId, dto);

            return Ok(res);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsOfTheProduct([FromRoute] Guid productId)
        {
            var reviews = await reviewsService.ReviewsOfProductAsync(productId);
            
            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                UserName = r.User?.AppUser?.UserName ?? "Anonymous",
                Rating = r.Rating,
                ReviewText = r.Review,
                CreatedAt = r.CreatedAt
            }).ToList();

            return Ok(reviewDtos);
        }
    }
}
