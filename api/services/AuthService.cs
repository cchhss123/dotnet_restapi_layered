using api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace api.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtService _jwtService;

        public AuthService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public Task<(string? Token, string? ErrorMessage)> LoginAsync(ApiAuthDto authDto)
        {
            // Simulate API account validation (in a real app, query from database)
            if (authDto.Account == "api" && authDto.Password == "test")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"), // Example User ID
                    new Claim(ClaimTypes.Name, authDto.Account),
                    new Claim(ClaimTypes.Role, "Admin") // Example Role
                };

                var jwtToken = _jwtService.GenerateToken(claims);
                return Task.FromResult<(string?, string?)>((jwtToken, null));
            }
            else
            {
                return Task.FromResult<(string?, string?)>((null, "Invalid credentials"));
            }
        }
    }
}