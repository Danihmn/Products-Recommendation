using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsRecommendations.Models;

namespace ProductsRecommendations.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(p => p.Category)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(p => p.Summary)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnType("text")
            .IsRequired();
    }
}