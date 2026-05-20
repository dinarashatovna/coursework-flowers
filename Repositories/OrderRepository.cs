using GardenWalk.Data;
using GardenWalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GardenWalk.Repositories;

/// <summary>
/// Реализация репозитория заказов. Обеспечивает сохранение оформленных
/// заказов в базе данных и предоставляет возможность получить заказ
/// по идентификатору вместе с позициями и адресом доставки.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public OrderRepository(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
