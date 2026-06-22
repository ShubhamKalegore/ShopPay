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

}
