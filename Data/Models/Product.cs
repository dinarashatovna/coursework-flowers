namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Товар» (букет или цветочная композиция).
/// Имеет связь «многие к одному» с категорией <see cref="Category"/>,
/// связь «один ко многим» с дополнительными изображениями <see cref="ProductImage"/>,
/// связь «многие ко многим» с тегами <see cref="Tag"/>.
/// </summary>
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool IsFeatured { get; set; }

    public string MainImage { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<ProductImage> Gallery { get; set; } = new List<ProductImage>();

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
