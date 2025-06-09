using api.Models;
using api.Services; // Corrected namespace for DatabaseService
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseService _dbService;

        public UserRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _dbService.QueryAsync(
                "SELECT id, name, email FROM users",
                reader => new UserDto(reader.GetString(1), reader.GetString(2)) // Assuming id is not part of UserDto for this context or handled differently
            );
        }

        public async Task<UserDto?> GetUserByIdAsync(long id)
        {
            return await _dbService.QuerySingleAsync(
                "SELECT id, name, email FROM users WHERE id = @id",
                reader => new UserDto(reader.GetString(1), reader.GetString(2)),
                new SqlParameter("@id", id)
            );
        }

        public async Task<long> CreateUserAsync(UserDto user)
        {
            string sql = "INSERT INTO users (name, email) VALUES (@name, @email); SELECT SCOPE_IDENTITY();";
            var newUserId = await _dbService.ExecuteScalarAsync(sql,
                new SqlParameter("@name", user.Name),
                new SqlParameter("@email", user.Email));
            return newUserId;
        }

        public async Task<bool> UpdateUserAsync(long id, UserDto user)
        {
            string sql = "UPDATE users SET name = @name, email = @email WHERE id = @id";
            var rowsAffected = await _dbService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@name", user.Name),
                new SqlParameter("@email", user.Email),
                new SqlParameter("@id", id));
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            string sql = "DELETE FROM users WHERE id = @id";
            var rowsAffected = await _dbService.ExecuteNonQueryAsync(sql, new SqlParameter("@id", id));
            return rowsAffected > 0;
        }
    }
}