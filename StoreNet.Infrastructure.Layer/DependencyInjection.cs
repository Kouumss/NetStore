using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;
using StoreNet.Infrastructure.Layer.Repositories;

namespace StoreNet.Infrastructure.Layer;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddSingleton<IEntityIdGenerator, UlidEntityIdGenerator>();

        return services;
    }
}
