using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Infrastructure.Layer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


         
            // Customer and Addresses (one-to-many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // index unique pour l'adresse de facturation (AddressType = Billing)
            modelBuilder.Entity<Address>()
                .HasIndex(a => new { a.CustomerId, a.AddressType })
                .IsUnique()
                .HasFilter("[AddressType] = 1"); // 1 correspond à "Billing" dans l'énumération AddressType (Billing)

            // index (non unique) pour les adresses de livraison
            modelBuilder.Entity<Address>()
                .HasIndex(a => new { a.CustomerId, a.AddressType })
                .HasFilter("[AddressType] = 2"); // 2 correspond à "Shipping" dans l'énumération AddressType (Shipping)

            // Customer and Orders
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);


            // Order and Billing Address
            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order and Shipping Address
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order and OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            // Order and Payment (one-to-one relationship)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order and Cancellation (one-to-one relationship)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cancellation)
                .WithOne(c => c.Order)
                .HasForeignKey<Cancellation>(c => c.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Refund and Payment (one-to-one relationship)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Refund)
                .WithOne(r => r.Payment)
                .HasForeignKey<Refund>(r => r.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Refund and Cancellation (one-to-one relationship)
            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Cancellation)
                .WithOne(c => c.Refund)
                .HasForeignKey<Refund>(r => r.CancellationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
