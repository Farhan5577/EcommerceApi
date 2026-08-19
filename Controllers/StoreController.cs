using EcommerceApi.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EcommerceApi.Service.Interface;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class StoreController(IStoreService storeService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Createstore([FromForm] StoreDto dto)
        {
            try
            {
                var usingIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(usingIdString))
                    return Unauthorized(new { Message = "Unauthenticated user" });

                var UserId = Guid.Parse(usingIdString);
                var result = await storeService.CreateStore(dto, UserId);

                return Ok(new { Message = "The store has been successfully created!", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpGet ("My-Store")]
        [Authorize]
        public async Task<IActionResult> GetMyStore()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var userId = Guid.Parse(userIdString);
            var store = await storeService.GetStoreByUserId(userId);

            if (store == null)
                return NotFound();

            return Ok(store);
        }
    }
}
