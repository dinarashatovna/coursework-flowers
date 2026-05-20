using GardenWalk.Data.Models;

namespace GardenWalk.Repositories;

/// <summary>
/// Интерфейс репозитория товаров. Определяет операции чтения данных
/// о товарах из базы данных и инкапсулирует логику доступа
/// к сущности <see cref="Product"/>.
/// </summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(string? categorySlug = null, string? search = null);

    Task<Product?> GetBySlugAsync(string slug);
}
