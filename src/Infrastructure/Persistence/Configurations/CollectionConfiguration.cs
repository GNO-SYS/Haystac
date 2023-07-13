using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haystac.Infrastructure.Persistence.Configurations;

public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder.HasKey(c => c.Uuid);

        builder.HasMany(c => c.Items)
               .WithOne(i => i.Collection)
               .HasForeignKey(i => i.CollectionId);
    }
}
