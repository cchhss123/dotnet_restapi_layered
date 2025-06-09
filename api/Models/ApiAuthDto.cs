namespace api.Models
{
    /// <summary>
    /// API 認證資料傳輸物件 (DTO)。
    /// </summary>
    /// <param name="Account">使用者帳號。</param>
    /// <param name="Password">使用者密碼。</param>
    public record ApiAuthDto(string Account, string Password);
}