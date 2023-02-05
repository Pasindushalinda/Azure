using AzureFunctionFundamentals.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunctionFundamentals.Data;

public class AzureFunctionDbContext : DbContext
{
    public AzureFunctionDbContext(DbContextOptions<AzureFunctionDbContext> options) : base(options)
    {
    }

    public DbSet<SalesRequest> SalesRequests { get; set; }
    public DbSet<GroceryItem> GroceryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SalesRequest>(entity => { entity.HasKey(s => s.Id); });
        modelBuilder.Entity<GroceryItem>(entity => { entity.HasKey(s => s.Id); });
    }
}