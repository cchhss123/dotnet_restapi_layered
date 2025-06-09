using api.Models; // 引用 User 模型
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 如果 User.cs 中沒有使用 [Table("users")] 屬性，可以在這裡設定：
            // modelBuilder.Entity<User>().ToTable("users");

            // 可以在這裡加入更複雜的模型設定，例如：
            // modelBuilder.Entity<User>()
            //     .HasIndex(u => u.Email)
            //     .IsUnique();
        }
    }
}