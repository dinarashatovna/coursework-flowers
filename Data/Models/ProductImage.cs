namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Изображение товара». Представляет одно дополнительное изображение
/// галереи, относящееся к товару. Имеет связь «многие к одному»
/// с сущностью <see cref="Product"/>.
/// </summary>
public class ProductImage
{
    public int Id { get; set; }

    public string Url { get; set; } = string.Empty;

    public string Alt { get; set; } = string.Empty;

    public int Order { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
