using EcommerceApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Service.Interface;
using EcommerceApi.Options.Exceptions;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] OrderDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await orderService.CreateOrder(dto, userId);
            return Ok(new { Message = "Checkout successfully created", Data = result });
        }

        [HttpGet("my-order")]
        public async Task<IActionResult> GetOrders()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await orderService.GetMyOrder(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await orderService.GetOrderById(id, userId);

            if (result == null) return NotFound();

            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await orderService.UpdateOrderStatus(id, dto, userId);

            return Ok(new {Message = "Order status successfully updated!", Data = result });
        }
    }
}
