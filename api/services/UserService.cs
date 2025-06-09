using api.Models;
using api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<(UserDto? User, string? ErrorMessage)> CreateUserAsync(UserDto user)
        {
            // Basic validation example
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            {
                return (null, "Name and Email cannot be empty.");
            }
            // More complex validation (e.g., email format, check if email exists) could be added here.

            var newUserId = await _userRepository.CreateUserAsync(user);
            if (newUserId > 0)
            {
                // Assuming UserDto doesn't have an ID, or we fetch the created user again if it does.
                // For simplicity, returning the input DTO. A real app might fetch the entity.
                return (new UserDto(user.Name, user.Email), null); // Or create a new DTO with the ID: new UserDto(newUserId, user.Name, user.Email) if DTO supports ID
            }
            return (null, "Failed to create user.");
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateUserAsync(long id, UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            {
                return (false, "Name and Email cannot be empty.");
            }

            var success = await _userRepository.UpdateUserAsync(id, user);
            if (success)
            {
                return (true, null);
            }
            return (false, "User not found or failed to update.");
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteUserAsync(long id)
        {
            var success = await _userRepository.DeleteUserAsync(id);
            if (success)
            {
                return (true, null);
            }
            return (false, "User not found or failed to delete.");
        }
    }
}