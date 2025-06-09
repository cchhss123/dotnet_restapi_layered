using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace api.Services
{
    public class DatabaseService : IDisposable
    {
        private readonly Lazy<SqlConnection> _connection;
        private bool _disposed = false;

        public DatabaseService(IConfiguration config)
        {
            _connection = new Lazy<SqlConnection>(() => new SqlConnection(config.GetConnectionString("DefaultConnection")));
        }

        private async Task EnsureConnectionOpenAsync()
        {
            if (_connection.Value.State != ConnectionState.Open)
            {
                await _connection.Value.OpenAsync();
            }
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> mapper, params SqlParameter[] parameters)
        {
            await EnsureConnectionOpenAsync();
            var results = new List<T>();

            using (var cmd = CreateCommand(sql, parameters))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    results.Add(mapper(reader));
                }
            }

            return results;
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, Func<SqlDataReader, T> mapper, params SqlParameter[] parameters) where T : class
        {
            await EnsureConnectionOpenAsync();

            using (var cmd = CreateCommand(sql, parameters))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.ReadAsync() ? mapper(reader) : null;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
        {
            await EnsureConnectionOpenAsync();

            using (var cmd = CreateCommand(sql, parameters))
            {
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<long> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
        {
            await EnsureConnectionOpenAsync();

            using (var cmd = CreateCommand(sql, parameters))
            {
                var result = await cmd.ExecuteScalarAsync();
                return result != null ? Convert.ToInt64(result) : 0;
            }
        }

        private SqlCommand CreateCommand(string sql, SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(sql, _connection.Value);
            if (parameters?.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection?.Value.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    } // End of DatabaseService class
} // End of api.Services namespace
