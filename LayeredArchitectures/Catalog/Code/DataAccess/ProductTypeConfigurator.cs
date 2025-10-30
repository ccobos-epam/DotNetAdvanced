using DomainEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess;

public class ProductTypeConfigurator : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Products", t => t.HasCheckConstraint("CK_Product_Ammount_AlwaysPositive", "Ammount >= 0"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Description).IsRequired(false);
        builder.Property(x => x.Price).IsRequired(true);
        builder.Property(x => x.Ammount).IsRequired(true);
        builder.Property(x => x.ImageUrl).IsRequired(false);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.ProductsInCategory)
            .HasForeignKey(b => b.CategoryId)
            .IsRequired(true);
    }
}
