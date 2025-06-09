using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 取得所有使用者。
        /// </summary>
        /// <returns>包含所有使用者列表的成功回應。</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new { message = "Success", data = users });
        }

        /// <summary>
        /// 根據 ID 取得單一使用者。
        /// </summary>
        /// <param name="id">使用者 ID。</param>
        /// <returns>如果找到使用者，則返回包含使用者資料的成功回應；否則返回 404 Not Found。</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = new { code = 404, message = "User not found", details = $"No user exists with ID {id}" } });
            }
            return Ok(new { message = "Success", data = user });
        }

        /// <summary>
        /// 建立一個新使用者。
        /// </summary>
        /// <param name="userDto">包含新使用者資料的 DTO。</param>
        /// <returns>如果成功建立使用者，則返回 201 Created 回應；否則返回 400 Bad Request。</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (createdUser, errorMessage) = await _userService.CreateUserAsync(userDto);

            if (errorMessage != null)
            {
                // Consider different status codes based on error type if available
                return BadRequest(new { error = new { code = 400, message = errorMessage } });
            }
            // Assuming createdUser contains the ID if UserDto is updated to include it, or use a different DTO for response.
            return CreatedAtAction(nameof(GetUserById), new { id = 0 /* Replace with actual ID if available */ }, new { message = "User created successfully", data = createdUser });
        }

        /// <summary>
        /// 更新現有使用者。
        /// </summary>
        /// <param name="id">要更新的使用者 ID。</param>
        /// <param name="userDto">包含更新使用者資料的 DTO。</param>
        /// <returns>如果成功更新使用者，則返回成功回應；否則返回 404 Not Found 或 400 Bad Request。</returns>
        [HttpPut("{id}")]
        [Authorize] // Protect this endpoint
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, errorMessage) = await _userService.UpdateUserAsync(id, userDto);

            if (!success)
            {
                // Distinguish between "not found" and "failed to update" if possible
                return NotFound(new { error = new { code = 404, message = errorMessage ?? "User not found or failed to update." } });
            }
            return Ok(new { message = "User updated successfully" });
        }

        /// <summary>
        /// 刪除指定 ID 的使用者。
        /// </summary>
        /// <param name="id">要刪除的使用者 ID。</param>
        /// <returns>如果成功刪除使用者，則返回成功回應；否則返回 404 Not Found。</returns>
        [HttpDelete("{id}")]
        [Authorize] // Protect this endpoint
        public async Task<IActionResult> DeleteUser(long id)
        {
            var (success, errorMessage) = await _userService.DeleteUserAsync(id);

            if (!success)
            {
                return NotFound(new { error = new { code = 404, message = errorMessage ?? "User not found or failed to delete." } });
            }
            return Ok(new { message = "User deleted successfully" });
        }
    }
}