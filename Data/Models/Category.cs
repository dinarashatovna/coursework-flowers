namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Категория товаров». Объединяет товары схожего назначения
/// (например, «Розы», «Полевые букеты»). Имеет связь «один ко многим»
/// с сущностью <see cref="Product"/>.
/// </summary>
public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string IconEmoji { get; set; } = "🌸";

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
