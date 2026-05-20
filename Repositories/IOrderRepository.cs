using GardenWalk.Data.Models;

namespace GardenWalk.Repositories;

/// <summary>
/// Интерфейс репозитория заказов. Определяет операции создания
/// и чтения данных о заказах покупателей.
/// </summary>
public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
}
