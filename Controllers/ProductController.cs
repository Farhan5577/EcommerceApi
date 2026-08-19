using EcommerceApi.DTOs;
using EcommerceApi.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAllProduct();
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
            try
            {
                var UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await productService.CreateProduct(dto, UserId);

                return Ok(new { Message = "Product successfully added!", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] ModProductDto dto )
        {
            try
            {
                var userID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await productService.UpdateProduct(id, dto, userID);

                return Ok(new { Message = "Product Successfully Update!", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
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
