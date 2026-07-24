using Microsoft.Extensions.DependencyInjection;
using ShopPay.Application.Interfaces;
using ShopPay.Application.Mappings;
using ShopPay.Application.Services;
using ShopPay.Infrastructure.Services;

namespace ShopPay.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(configuration => configuration.AddProfile<MapperProfile>());
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IStripeCustomerService, StripeCustomerService>();
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<IEmailService, EmailService>();


        return services;
    }
}
