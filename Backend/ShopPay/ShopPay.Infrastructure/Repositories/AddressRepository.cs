using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class AddressRepository
    : GenericRepository<Address, int>,
      IAddressRepository
{
    public AddressRepository(AppDbContext context)
        : base(context)
    {
    }
}