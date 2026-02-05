using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) 
            : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure InventoryItem table
            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Quantity)
                    .HasDefaultValue(0);

                entity.Property(e => e.ReorderLevel)
                    .HasDefaultValue(10);

                entity.Property(e => e.UnitPrice)
                    .HasPrecision(18, 2);

                entity.Property(e => e.SupplierName)
                    .IsRequired();

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValue(DateTime.UtcNow);

                entity.Property(e => e.UpdatedDate)
                    .HasDefaultValue(DateTime.UtcNow);

                // Create indexes for common queries
                entity.HasIndex(e => e.ItemName);
                entity.HasIndex(e => e.Quantity);
            });
        }
    }
}
