using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using ProductService.API.Entities;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
    public DbSet<ProductAttributeValue> ProductAttributeValues => Set<ProductAttributeValue>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        // Product
        model.Entity<Product>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SKU).IsUnique();
            b.HasIndex(x => x.Slug);
            b.Property(x => x.ProductName).IsRequired().HasMaxLength(250);
            b.HasOne(x => x.Brand).WithMany(b => b.Products).HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.Category).WithMany(c => c.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        // Variant
        model.Entity<ProductVariant>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.VariantSKU).IsUnique(false); // nếu bạn muốn unique bật true
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
            b.HasOne(v => v.Product).WithMany(p => p.Variants).HasForeignKey(v => v.ProductId);
        });

        // Attribute & values
        model.Entity<ProductAttribute>(b => { b.HasKey(x => x.Id); b.Property(x => x.Name).IsRequired().HasMaxLength(120); });
        model.Entity<ProductAttributeValue>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(v => v.ProductVariant).WithMany(p => p.AttributeValues).HasForeignKey(v => v.ProductVariantId);
            b.HasOne(v => v.Attribute).WithMany(a => a.Values).HasForeignKey(v => v.AttributeId);
        });

        // Images
        model.Entity<ProductImage>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(i => i.Product).WithMany(p => p.Images).HasForeignKey(i => i.ProductId);
            b.HasIndex(i => new { i.ProductId, i.SortOrder });
        });

        // Warehouse & inventory
        model.Entity<Warehouse>(b => b.HasKey(x => x.Id));
        model.Entity<Inventory>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.ProductVariantId, x.WarehouseId }).IsUnique();
            b.HasOne(i => i.ProductVariant).WithMany(v => v.Inventories).HasForeignKey(i => i.ProductVariantId);
            b.HasOne(i => i.Warehouse).WithMany(w => w.Inventories).HasForeignKey(i => i.WarehouseId);
        });

        // Price history
        model.Entity<PriceHistory>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
            b.HasOne(ph => ph.ProductVariant).WithMany(v => v.PriceHistories).HasForeignKey(ph => ph.ProductVariantId);
        });

        // Tag many-to-many
        model.Entity<Tag>(b => { b.HasKey(x => x.Id); b.HasIndex(x => x.Name); });
        model.Entity<ProductTag>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.ProductId, x.TagId }).IsUnique();
            b.HasOne(pt => pt.Product).WithMany(p => p.ProductTags).HasForeignKey(pt => pt.ProductId);
            b.HasOne(pt => pt.Tag).WithMany(t => t.ProductTags).HasForeignKey(pt => pt.TagId);
        });

        // Optional: global query filter nếu AuditableEntity có IsDeleted
        // Uncomment nếu base class định nghĩa public bool IsDeleted { get; set; }
        // model.Entity<Product>().HasQueryFilter(p => !EF.Property<bool>(p, "IsDeleted"));
        // model.Entity<ProductVariant>().HasQueryFilter(v => !EF.Property<bool>(v, "IsDeleted"));
    }
}
