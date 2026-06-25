using Microsoft.EntityFrameworkCore;
using ShopPay.Domain.Entities;

namespace ShopPay.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Address => Set<Address>();
    public DbSet<StripeCustomer> StripeCustomers { get; set; }

    public DbSet<Billing> Billings { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Subscription> Subscriptions
    => Set<Subscription>();

    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Billing>()
            .HasOne(x => x.User)
            .WithMany(x => x.Billings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Billing>()
            .HasOne(x => x.Order)
            .WithMany(x => x.Billings)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Subscription>()
            .HasOne(x => x.User)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Invoice>()
            .HasOne(x => x.Subscription)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

