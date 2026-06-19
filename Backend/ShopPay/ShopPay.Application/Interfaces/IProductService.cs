using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();

    Task<ProductDto?> GetProductByIdAsync(int id);

    Task<ProductDto> CreateProductAsync(ProductDto productDto);

    Task<ProductDto?> UpdateProductAsync(int id, ProductDto productDto);

    Task DeleteProductAsync(int id);
}