using api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(long id);
        Task<(UserDto? User, string? ErrorMessage)> CreateUserAsync(UserDto user); // Returning tuple for better error handling or created user
        Task<(bool Success, string? ErrorMessage)> UpdateUserAsync(long id, UserDto user);
        Task<(bool Success, string? ErrorMessage)> DeleteUserAsync(long id);
    }
}