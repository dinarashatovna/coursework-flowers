namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Тег». Используется для классификации товаров по характеру
/// и поводу (например, «нежный», «весна», «монобукет»). Имеет связь
/// «многие ко многим» с сущностью <see cref="Product"/>.
/// </summary>
public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
