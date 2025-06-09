using api.Models;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IAuthService
    {
        Task<(string? Token, string? ErrorMessage)> LoginAsync(ApiAuthDto authDto);
    }
}