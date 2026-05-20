using GardenWalk.Data.Models;

namespace GardenWalk.Repositories;

/// <summary>
/// Интерфейс репозитория категорий. Определяет операции чтения данных
/// о категориях товаров из базы данных.
/// </summary>
public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync();
    Task<Category?> GetBySlugAsync(string slug);
}
