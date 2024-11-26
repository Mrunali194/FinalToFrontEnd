using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext {

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<UserDetails> userDetailsDb {get; set;}
    public DbSet<DrugsCart> drugCartDb {get; set;}
    public DbSet<SalesReport> drugSalesReportDb {get; set;}
    public DbSet<InventoryDetails> inventoryDetails{get; set;}
    public DbSet<OrderDetails> orderDetails{get; set;}
    public DbSet<SupplierDetails> supplierDetails{get; set;}

    public DbSet<TransactionDetails> transactionDetails{get;set;}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Prevent cascading deletes on the OrderDetails -> TransactionDetails relationship
        modelBuilder.Entity<TransactionDetails>()
            .HasOne(t => t.Order)
            .WithMany(o => o.Transactions)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict); // or .OnDelete(DeleteBehavior.SetNull)

        // Optionally, if you want to prevent cascading deletes in other relationships
        // (for example, if you have cascading delete on related entities)
        modelBuilder.Entity<OrderDetails>()
            .HasMany(o => o.Transactions)
            .WithOne(t => t.Order)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict); // or .OnDelete(DeleteBehavior.SetNull)
    }
}