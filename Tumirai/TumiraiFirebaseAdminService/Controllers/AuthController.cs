using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumiraiFirebaseAdminService.Services;

namespace TumiraiFirebaseAdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FirebaseAdminService _firebaseAdminService;

        public AuthController(FirebaseAdminService firebaseAdminService)
        {
            _firebaseAdminService = firebaseAdminService;
        }

        [HttpPost("set-custom-claims")]
        public async Task<IActionResult> SetCustomClaims([FromBody] SetCustomClaimsRequest request)
        {
            try
            {
                await _firebaseAdminService.SetCustomUserClaimsAsync(request.Uid, request.Claims);
                return Ok("Custom claims set successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error setting custom claims: {ex.Message}");
            }
        }

        [HttpGet("test")]
        public IActionResult TestApi()
        {
            return Ok(new { message = "API is working!" });
        }
    }

    public class SetCustomClaimsRequest
    {
        public string Uid { get; set; }
        public Dictionary<string, object> Claims { get; set; }
    }
}

