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
    }
}

