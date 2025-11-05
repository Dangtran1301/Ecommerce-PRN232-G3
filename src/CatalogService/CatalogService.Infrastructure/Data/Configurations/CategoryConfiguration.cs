using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CategoryName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.CategoryDescription)
                .HasMaxLength(1000);
            builder.HasIndex(c => c.CategoryName).IsUnique();
        }
    }
}