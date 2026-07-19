
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.AspNetCore;
using ShopPay.Application;
using ShopPay.Infrastructure;
using ShopPay.McpServer.Tools;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<ProductTools>();

builder.Services.AddSingleton<ProductTools>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapMcp();

app.Run();