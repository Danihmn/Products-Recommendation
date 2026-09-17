using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsRecommendations.Models;

namespace ProductsRecommendations.Data.Mappings;

public class RecommendationMapping : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        builder.ToTable("recommendations");

        builder.HasKey(r => r.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(r => r.Title)
            .HasColumnName("title")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(r => r.Category)
            .HasColumnName("category")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(r => r.Embedding)
            .HasColumnName("embedding")
            .HasColumnType("vector(1024)")
            .IsRequired();
    }
}