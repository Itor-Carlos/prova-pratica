using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(1000);
            entity.Property(x => x.Category).HasColumnName("category").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Price).HasColumnName("price").HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<int>().IsRequired();
            entity.Property(x => x.ImageUrl).HasColumnName("image_url").HasMaxLength(300);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

            entity.HasIndex(x => x.Category).HasDatabaseName("ix_products_category");
            entity.HasIndex(x => x.Status).HasDatabaseName("ix_products_status");
        });

        base.OnModelCreating(modelBuilder);
    }
}
