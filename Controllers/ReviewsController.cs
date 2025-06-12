using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Reviews>>> GetReviewsOfTheProduct([FromRoute] Guid productId)
        {
            var reviewsOfProduct = await reviewsService.ReviewsOfProductAsync(productId);

            return Ok(reviewsOfProduct);
        }
    }
}
