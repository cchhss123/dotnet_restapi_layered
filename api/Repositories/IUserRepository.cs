using api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(long id);
        Task<long> CreateUserAsync(UserDto user);
        Task<bool> UpdateUserAsync(long id, UserDto user);
        Task<bool> DeleteUserAsync(long id);
    }
}