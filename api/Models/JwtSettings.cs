namespace api.Models
{
    /// <summary>
    /// JWT 設定模型。
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 取得或設定 JWT 簽章金鑰。
        /// </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// 取得或設定 JWT 發行者。
        /// </summary>
        public string Issuer { get; set; } = string.Empty;
        /// <summary>
        /// 取得或設定 JWT 目標受眾。
        /// </summary>
        public string Audience { get; set; } = string.Empty;
        /// <summary>
        /// 取得或設定 JWT 過期時間（分鐘）。
        /// </summary>
        public int ExpiryMinutes { get; set; }
    }
}