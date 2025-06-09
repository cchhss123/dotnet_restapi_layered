namespace api.Models
{
    /// <summary>
    /// 使用者資料傳輸物件 (DTO)。
    /// </summary>
    /// <param name="Name">使用者名稱。</param>
    /// <param name="Email">使用者電子郵件。</param>
    public record UserDto(string Name, string Email);
}