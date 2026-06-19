using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface IProductRepository : IGenericRepository<Product, int>
{
}