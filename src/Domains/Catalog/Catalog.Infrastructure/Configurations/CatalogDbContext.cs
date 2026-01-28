using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Configurations
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(b =>
            {
                b.ToTable("product");

                b.HasKey(p => p.Id);
                b.Property(p => p.Id)
                   .HasColumnName("id")
                   .IsRequired();

                b.Property(p => p.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired();

                b.OwnsOne(p => p.Price, sa =>
                {
                    sa.Property(m => m.Amount)
                      .HasColumnName("price_amount")
                      .HasColumnType("numeric(18,2)")
                      .IsRequired();

                    sa.Property(m => m.Currency)
                      .HasColumnName("price_currency")
                      .HasMaxLength(10)
                      .IsRequired()
                      .HasDefaultValue("BRL");
                });

                b.Property(p => p.Stock)
                    .HasDefaultValue(0);

                b.HasCheckConstraint("CK_Product_PriceAmount", "price_amount > 0");
                b.HasCheckConstraint("CK_Product_Stock", "stock >= 0");

                b.HasIndex(p => p.Name).HasDatabaseName("idx_product_name");
                b.HasIndex(p => p.CategoryId).HasDatabaseName("idx_product_category_id");
            });

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("created_at") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("created_at").CurrentValue = DateTime.Now;
                    entry.Property("updated_at").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("created_at").IsModified = false;
                    entry.Property("updated_at").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
