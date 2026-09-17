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

        builder.Property(r => r.Title)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(r => r.Category)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(r => r.Embedding)
            .HasColumnType("vector(1024)")
            .IsRequired();
    }
}