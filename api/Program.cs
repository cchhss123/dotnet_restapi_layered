using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Security.Claims; // For Claims
using System.Text; // For Encoding
using Microsoft.IdentityModel.Tokens; // For SymmetricSecurityKey
using Microsoft.AspNetCore.Authentication.JwtBearer; // For JWT Bearer Authentication
using Microsoft.Extensions.Options;
using api.Models; // Add using for DTOs
using api.Services; // Add using for Services
using api.Repositories; // Add using for Repositories
using api.Middleware; // Add using for Middleware
using Microsoft.AspNetCore.DataProtection; // For Data Protection
using System.IO; // For Path operations
using api.Data; // For ApplicationDbContext
using Microsoft.EntityFrameworkCore; // For UseSqlServer and AddDbContext
using Microsoft.Extensions.Logging; // For ILogger

var builder = WebApplication.CreateBuilder(args);

// 加入 Configuration
builder.Services.AddSingleton(builder.Configuration);

// 註冊 Services and Repositories
builder.Services.AddScoped<DatabaseService>(); // Remains as it's used by UserRepository
builder.Services.AddSingleton<JwtService>(); // Remains as it's used by AuthService & JWT middleware

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 設定資料庫上下文 (DbContext)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 加入 Controller 服務
builder.Services.AddControllers();

// ====================================================================================
// ✅ 資料保護設定 (Data Protection Configuration)
// ====================================================================================
// 設定資料保護金鑰的儲存位置
// 在生產環境中，建議將金鑰儲存到持久化的位置，例如掛載的磁碟區或 Azure Key Vault。
// 同時，建議使用 X.509 憑證來加密靜態儲存的金鑰。
// 更多資訊請參考：https://docs.microsoft.com/aspnet/core/security/data-protection/configuration/overview
// 將金鑰儲存位置改為應用程式內容根目錄下的 DataProtection-Keys 資料夾
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys");
Directory.CreateDirectory(keysFolder); // 確保資料夾存在

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    // 在生產環境中，取消註解以下程式碼並提供有效的 X.509 憑證來加密金鑰
    // .ProtectKeysWithCertificate("thumbprint");
    ;

// ====================================================================================
// ✅ JWT 認證配置 (ASP.NET Core框架處理)
// ====================================================================================
// 註冊 JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// 獲取 JWT 相關設定 (從 appsettings.json)
// var jwtSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>().Value; // ASP0000 warning
// var secretKey = jwtSettings.Key ?? throw new ArgumentNullException(nameof(jwtSettings), "JWT Key is not configured in appsettings.json");
// var issuer = jwtSettings.Issuer ?? throw new ArgumentNullException(nameof(jwtSettings), "JWT Issuer is not configured in appsettings.json");
// var audience = jwtSettings.Audience ?? throw new ArgumentNullException(nameof(jwtSettings), "JWT Audience is not configured in appsettings.json");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // 直接從 IConfiguration 讀取 JWT 設定
    var jwtSettingsConfig = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSettingsConfig["Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT Key is not configured in appsettings.json");
    var issuer = jwtSettingsConfig["Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT Issuer is not configured in appsettings.json");
    var audience = jwtSettingsConfig["Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT Audience is not configured in appsettings.json");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

});

builder.Services.AddAuthorization(); // 啟用授權

// ====================================================================================

// 註冊 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "使用者管理 API，支援 CRUD 操作"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "請輸入 JWT token (格式：Bearer YOUR_TOKEN)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // 設定 Swagger 載入 XML 文件
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// 註冊 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

// ====================================================================================
// ✅ 啟用認證和授權中介軟體
// ====================================================================================
app.UseAuthentication();
app.UseAuthorization();
app.UseJwtAuthMiddleware(); // 使用自訂的 JWT 認證中介軟體
// ====================================================================================

// 啟用 Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
    options.RoutePrefix = "swagger"; // 設定 Swagger UI 在 `/swagger`
});

// Minimal API endpoints removed, logic moved to Controllers

// 對應 Controller 路由
app.MapControllers();

// 在應用程式啟動時執行資料庫遷移
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();

// DTOs moved to api/Models
