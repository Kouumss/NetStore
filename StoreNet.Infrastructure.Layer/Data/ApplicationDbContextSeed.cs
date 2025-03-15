using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Infrastructure.Layer.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger<ApplicationDbContextSeed> logger, IConfiguration configuration)
        {
            try
            {
                await SeedDataAsync<Customer>(context.Customers, "CustomersFilePath", context, logger, configuration);
                await SeedDataAsync<Category>(context.Categories, "CategoriesFilePath", context, logger, configuration);
                await SeedDataAsync<Product>(context.Products, "ProductsFilePath", context, logger, configuration);
                await SeedDataAsync<Order>(context.Orders, "OrdersFilePath", context, logger, configuration);
                await SeedDataAsync<Status>(context.Statuses, "OrderStatusesFilePath", context, logger, configuration); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while seeding the database.");
            }
        }

        private static async Task SeedDataAsync<TEntity>(DbSet<TEntity> dbSet, string configKey, ApplicationDbContext context, ILogger logger, IConfiguration configuration) where TEntity : class
        {
            try
            {
                if (!dbSet.Any())
                {
                    var relativePath = configuration.GetValue<string>($"SeedData:{configKey}");
                    if (string.IsNullOrEmpty(relativePath))
                    {
                        logger.LogError($"Relative path for {configKey} is null or empty.");
                        return;
                    }

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                    await using var stream = File.OpenRead(filePath);
                    var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(stream);

                    if (data?.Any() == true)
                    {
                        dbSet.AddRange(data);
                        await context.SaveChangesAsync();
                        logger.LogInformation($"{typeof(TEntity).Name} added successfully.");
                    }
                    else
                    {
                        logger.LogWarning($"No {typeof(TEntity).Name} found in the seed data.");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                logger.LogError(ex, "File not found: {FilePath}", configuration.GetValue<string>($"SeedData:{configKey}"));
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "JSON deserialization error for file: {FilePath}", configuration.GetValue<string>($"SeedData:{configKey}"));
            }
        }
    }
}
