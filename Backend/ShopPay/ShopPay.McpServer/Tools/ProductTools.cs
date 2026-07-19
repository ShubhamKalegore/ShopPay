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
}  