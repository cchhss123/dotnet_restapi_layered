using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 使用者登入。
        /// 測試帳密 {"account": "api", "password": "test"}
        /// </summary>
        /// <param name="authDto">包含使用者登入憑證的 DTO。</param>
        /// <returns>如果登入成功，則返回包含 JWT token 的成功回應；否則返回 401 Unauthorized。</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] ApiAuthDto authDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (token, errorMessage) = await _authService.LoginAsync(authDto);

            if (errorMessage != null)
            {
                return Unauthorized(new { error = new { code = 401, message = errorMessage } });
            }

            return Ok(new { message = "Login successful", token });
        }
    }
}