using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;

namespace StoreNet.Infrastructure.Layer.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByCustomerAsync(string customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return customer?.Orders.ToList() ?? new List<Order>();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}
