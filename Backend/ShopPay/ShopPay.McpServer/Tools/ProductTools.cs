using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.McpServer.Tools;

[McpServerToolType]
public class ProductTools
{
    private readonly IProductService _productService;

    public ProductTools(IProductService productService)
    {
        _productService = productService;
    }


    [McpServerTool]
    [Description("Returns all products.")]
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        return await _productService.GetAllProductsAsync();
    }

    [McpServerTool]
    [Description("Returns a product by its ID.")]
    public async Task<ProductDto?> GetProduct(int id)
    {
        return await _productService.GetProductByIdAsync(id);
    }

    [McpServerTool]
    [Description("Creates a new product.")]
    public async Task<ProductDto> CreateProduct(ProductDto productDto)
    {
        return await _productService.CreateProductAsync(productDto);
    }

    [McpServerTool]
    [Description("Updates an existing product.")]
    public async Task<ProductDto?> UpdateProduct(
    int id,
    ProductDto productDto)
    {
        return await _productService.UpdateProductAsync(id, productDto);
    }

    [McpServerTool]
    [Description("Deletes a product by its ID.")]
    public async Task DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
    }
}  