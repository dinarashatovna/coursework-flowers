using GardenWalk.Data;
using GardenWalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GardenWalk.Repositories;

/// <summary>
/// Реализация репозитория категорий. Возвращает данные о категориях
/// товаров, отсортированные в алфавитном порядке.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public CategoryRepository(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }
}
