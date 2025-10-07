using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Data.Configurations
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

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(255);
            builder.HasIndex(c => c.CategoryName).IsUnique();
        }
    }
}
