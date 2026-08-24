using EcommerceApi.DTOs;
using EcommerceApi.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EcommerceApi.Options.Exceptions;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await productService.GetAllProduct(search, sortBy, pageNumber, pageSize);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var products = await productService.GetAllProductById(id);
            if (products == null) return NotFound();
            return Ok(products);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductDto dto)
        {
            var UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await productService.CreateProduct(dto, UserId);

            return Ok(new { Message = "Product successfully added!", Data = result });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] ModProductDto dto )
        {
            var userID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await productService.UpdateProduct(id, dto, userID);

            return Ok(new { Message = "Product Successfully Update!", Data = result });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await productService.DeleteProduct(id, userId);

            if (!result) return BadRequest();

            return Ok(new {Message = "Product successfully deleted." });
        }
    }
}
