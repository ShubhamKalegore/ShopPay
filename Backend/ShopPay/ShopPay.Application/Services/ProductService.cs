using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        _logger.LogInformation("Fetching all products");

        var products =
            await _productRepository.GetAllAsync();

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching product with id {ProductId}",
            id);

        var product =
            await _productRepository.GetByIdAsync(id);

        if (product is null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(
        ProductDto productDto)
    {
        _logger.LogInformation(
            "Creating product {ProductName}",
            productDto.Name);

        var now = DateTime.UtcNow;

        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateProductAsync(
        int id,
        ProductDto productDto)
    {
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product is null)
            return null;

        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.StockQuantity = productDto.StockQuantity;
        product.UpdatedAt = DateTime.UtcNow;

        _productRepository.Update(product);

        await _productRepository.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product is null)
            return;

        _productRepository.Delete(product);

        await _productRepository.SaveChangesAsync();
    }
}