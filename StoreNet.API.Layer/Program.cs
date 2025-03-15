using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreNet.Application.Layer;
using StoreNet.Infrastructure.Layer;
using StoreNet.Infrastructure.Layer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This will use the property names as defined in the C# model
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<ApplicationDbContextSeed>>();
    var configuration = services.GetRequiredService<IConfiguration>();

    await context.Database.MigrateAsync();
    await ApplicationDbContextSeed.SeedAsync(context, logger, configuration);
}
catch (Exception ex)
{
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
}

app.Run();
