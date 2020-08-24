using Jelly.Models.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Jelly.Models.Database
{
    public class JellyContext:DbContext
    {
        /// <summary>
        /// 构造函数 调用父类构造函数
        /// </summary>
        /// <param name="options"></param>
        public JellyContext(DbContextOptions<JellyContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostConfiguration());
        }
        public DbSet<Post> Posts { get; set; }

    }
}