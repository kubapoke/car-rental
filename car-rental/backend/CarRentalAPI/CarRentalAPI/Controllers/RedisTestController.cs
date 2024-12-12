using CarRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    public class RedisTestController : ControllerBase
    {
        private readonly RedisCacheService _redisCacheService;

        public RedisTestController(RedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            await _redisCacheService.SetValueAsync(key, value);
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetValue(string key)
        {
            var value = await _redisCacheService.GetValueAsync(key);
            return value != null ? Ok(value) : NotFound("Key not found.");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteValue(string key)
        {
            await _redisCacheService.DeleteKeyAsync(key);
            return Ok();
        }
    }
}