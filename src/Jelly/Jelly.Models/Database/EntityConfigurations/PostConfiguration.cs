using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jelly.Models.Database.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Author).HasMaxLength(50);
            builder.Property(x => x.Title).HasMaxLength(500);
        }
    }
}