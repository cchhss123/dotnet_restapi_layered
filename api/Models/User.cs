using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("users")] // 明確指定資料表名稱，與 init-db.sql 一致
    public class User
    {
        [Key] // 標示為主鍵
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 對應 IDENTITY
        public long Id { get; set; }

        [Required] // 對應 NOT NULL
        [StringLength(20)]
        public required string Name { get; set; }

        [Required] // 對應 NOT NULL
        [StringLength(50)]
        [EmailAddress]
        public required string Email { get; set; }

        // 假設未來可能有密碼欄位，但 init-db.sql 中沒有，所以暫不加入
        // public string PasswordHash { get; set; } 
    }
}