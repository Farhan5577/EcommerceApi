using EcommerceApi.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EcommerceApi.Service.Interface;
using EcommerceApi.Options.Exceptions;

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
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserId = Guid.Parse(userIdString);
            var result = await storeService.CreateStore(dto, UserId);

            return Ok(new { Message = "The store has been successfully created!", Data = result });

        }

        [HttpGet ("My-Store")]
        [Authorize]
        public async Task<IActionResult> GetMyStore()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdString);
            var store = await storeService.GetStoreByUserId(userId);
            return Ok(store);
        }
    }
}
