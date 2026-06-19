using Microsoft.Extensions.DependencyInjection;
using ShopPay.Application.Interfaces;
using ShopPay.Application.Mappings;
using ShopPay.Application.Services;

namespace ShopPay.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(configuration => configuration.AddProfile<MapperProfile>());
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}
