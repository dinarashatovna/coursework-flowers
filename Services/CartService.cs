using GardenWalk.Data.Models;

namespace GardenWalk.Services;

/// <summary>
/// Реализация сервиса пользовательской корзины, область действия которого
/// ограничена текущим соединением Blazor Server. Поддерживает уведомление
/// подписчиков об изменениях состояния посредством события <see cref="OnChange"/>.
/// </summary>
public class CartService
{
    private readonly List<CartLine> _lines = new();

    public IReadOnlyList<CartLine> Lines => _lines;

    public int ItemsCount => _lines.Sum(x => x.Quantity);

    public decimal Total => _lines.Sum(x => x.UnitPrice * x.Quantity);

    public event Action? OnChange;

    public void Add(Product product, int quantity = 1)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity < 1) quantity = 1;

        var existing = _lines.FirstOrDefault(x => x.ProductId == product.Id);
        if (existing is null)
        {
            _lines.Add(new CartLine
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                MainImage = product.MainImage,
                UnitPrice = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            existing.Quantity += quantity;
        }

        OnChange?.Invoke();
    }

    public void Remove(int productId)
    {
        var line = _lines.FirstOrDefault(x => x.ProductId == productId);
        if (line is not null)
        {
            _lines.Remove(line);
            OnChange?.Invoke();
        }
    }

    public void SetQuantity(int productId, int quantity)
    {
        var line = _lines.FirstOrDefault(x => x.ProductId == productId);
        if (line is null) return;

        if (quantity <= 0)
        {
            _lines.Remove(line);
        }
        else
        {
            line.Quantity = quantity;
        }

        OnChange?.Invoke();
    }

    public void Clear()
    {
        _lines.Clear();
        OnChange?.Invoke();
    }
}

/// <summary>
/// Позиция корзины, содержащая сведения о добавленном товаре
/// и его количестве.
/// </summary>
public class CartLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public string MainImage { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}
