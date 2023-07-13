using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haystac.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(i => i.Uuid);

        builder.HasOne(i => i.Collection)
               .WithMany(c => c.Items)
               .HasForeignKey(e => e.CollectionId);
    }
}
