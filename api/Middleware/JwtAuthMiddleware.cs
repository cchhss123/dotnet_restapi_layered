using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;

namespace api.Middleware
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 檢查是否是 401 Unauthorized 或 403 Forbidden 狀態碼
            // 並且回應尚未開始寫入
            context.Response.OnStarting(async () =>
            {
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        error = new
                        {
                            code = 401,
                            message = "Unauthorized",
                            details = "Authentication required. Please provide a valid JWT token."
                        }
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        error = new
                        {
                            code = 403,
                            message = "Forbidden",
                            details = "You do not have permission to access this resource."
                        }
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            await _next(context);
        }
    }

    public static class JwtAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthMiddleware>();
        }
    }
}