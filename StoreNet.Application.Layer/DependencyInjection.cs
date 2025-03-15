using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using StoreNet.Application.Layer.Factories;
using StoreNet.Application.Layer.Services;
using StoreNet.Application.Services;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Application.Layer;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly); 


        services.AddScoped<IEntityFactory, EntityFactory>();

        return services;
    }
}
