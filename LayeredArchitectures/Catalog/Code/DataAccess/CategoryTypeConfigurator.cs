using DomainEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess;

public class CategoryTypeConfigurator : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.ImageUrl).IsRequired(false);

        builder.HasOne(child => child.ParentCategory)
            .WithMany(parent => parent.PossibleChildCategories)
            .HasForeignKey(child => child.ParentCategoryId)
            .IsRequired(false);

        builder.HasMany(c => c.ProductsInCategory)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);
    }
}
