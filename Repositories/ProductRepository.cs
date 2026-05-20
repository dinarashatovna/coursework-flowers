using GardenWalk.Data;
using GardenWalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GardenWalk.Repositories;

/// <summary>
/// Реализация репозитория товаров. Использует фабрику контекстов
/// <see cref="IDbContextFactory{TContext}"/> для создания краткоживущих
/// контекстов EF Core под каждый запрос.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ProductRepository(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(string? categorySlug = null, string? search = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        IQueryable<Product> query = db.Products
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            query = query.Where(p => p.Category.Slug == categorySlug);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(s) ||
                p.ShortDescription.ToLower().Contains(s));
        }

        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .Include(p => p.Gallery.OrderBy(g => g.Order))
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }
}
