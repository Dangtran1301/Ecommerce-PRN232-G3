using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.BrandName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(b => b.BrandDescription)
                .HasMaxLength(1000);

            builder.Property(b => b.LogoUrl)
                .HasMaxLength(255);

            builder.Property(b => b.WebsiteUrl)
                .HasMaxLength(500);

            builder.HasIndex(b => b.BrandName).IsUnique();
        }
    }
}
