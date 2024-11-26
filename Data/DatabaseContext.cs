using Demo.Model;
using Microsoft.EntityFrameworkCore;
namespace Demo.Database;
public class DatabaseContext : DbContext {

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<UserDetails> UserDetails {get; set;}
    public DbSet<DrugsCart> DrugsCarts {get; set;}
    public DbSet<SalesReport> SalesReports {get; set;}
    public DbSet<DrugDetails> DrugDetails{get; set;}
    public DbSet<OrderDetails> OrderDetails{get; set;}
    public DbSet<SupplierDetails> SupplierDetails{get; set;}
    public DbSet<CartItem> CartItems{get;set;}
    public DbSet<TransactionDetails> TransactionDetails{get;set;}
    public DbSet<OrderItem> OrderItems{get;set;}
    
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalesReport>()
            .HasOne(s => s.Drug)
            .WithMany() // Adjust according to your model relationships
            .HasForeignKey(s => s.DrugId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure one-to-many relationship (one supplier can have many drugs)
            // modelBuilder.Entity<SupplierDetails>()
            // .HasMany(s => s.Drugs)
            // .WithOne(d => d.Supplier)
            // .HasForeignKey(d => d.SupplierId);

            // modelBuilder.Entity<DrugDetails>()
            // .HasOne(d => d.Supplier)
            // .WithMany(s => s.Drugs)
            // .HasForeignKey(d => d.SupplierId)
            // .OnDelete(DeleteBehavior.Cascade);
            
        // // Configure one-to-one relationship between User and Cart
        //     modelBuilder.Entity<UserDetails>()
        //     .HasOne(u => u.Cart)  // Each User has one Cart
        //     .WithOne(c => c.User)  // Each Cart has one User
        //     .HasForeignKey<DrugsCart>(c => c.UserId) // Foreign key in Cart referring to UserId
        //     .OnDelete(DeleteBehavior.Cascade);  // Cascade delete (optional)
    

            // Optionally, set table names if needed (for custom naming conventions)
            modelBuilder.Entity<UserDetails>().ToTable("Users");
            modelBuilder.Entity<DrugsCart>().ToTable("Carts");
            modelBuilder.Entity<SalesReport>().ToTable("SalesReports");
            modelBuilder.Entity<DrugDetails>().ToTable("Drugs");
            modelBuilder.Entity<OrderDetails>().ToTable("Orders");
            modelBuilder.Entity<SupplierDetails>().ToTable("Suppliers");
            modelBuilder.Entity<CartItem>().ToTable("CartItems");
            modelBuilder.Entity<TransactionDetails>().ToTable("Transactions");


      }
}
