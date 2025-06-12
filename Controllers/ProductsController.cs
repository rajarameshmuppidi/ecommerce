using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Services;
using EcommercePlatform.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly UserManager<IdentityUser> usermanager;

        public ProductsController(IProductService productService, UserManager<IdentityUser> usermanager)
        {
            this.productService = productService;
            this.usermanager = usermanager;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            var res = await productService.CreateProductAsync(product.SellerId, product);
            return Ok(res);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductUpdateDto dto)
        {
            var res = await productService.UpdateProductAsync(id, dto);
            if (res) return Ok("product updated successfully");
            else return BadRequest("Failed to Update");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product Not Found");
            else return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GellAllProductsAsync([FromQuery] string? searchString, [FromQuery] float? minPrice, [FromQuery] float? maxPrice, [FromQuery] bool reverse, [FromQuery] int? pageSize, [FromQuery] int? pageNumber, [FromQuery] string? sellerId)
        {
            var parameters = new ProductParameters();
            if(searchString != null) parameters.searchString = searchString;
            if (minPrice != null) parameters.minPrice = minPrice ?? 0;
            if (maxPrice != null) parameters.maxPrice = maxPrice ?? float.MaxValue;
            if(reverse) parameters.reverse = reverse;
            if(pageSize!=null) parameters.pageSize = pageSize ?? 0;
            if(pageNumber!=null) parameters.pageNumber = pageNumber ?? 0;
            if (sellerId != null) parameters.sellerId = sellerId;

            var res = await productService.GetAllProductsAsync(parameters);
            return Ok(res);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductByIdAsync([FromRoute] Guid productId)
        {
            var res = await productService.DeleteByIdAsync(productId);
            return Ok(res);
        }

    }
}
