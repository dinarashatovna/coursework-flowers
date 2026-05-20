namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Заказ покупателя». Имеет связь «один к одному»
/// с адресом доставки <see cref="ShippingAddress"/> и связь
/// «один ко многим» с позициями заказа <see cref="OrderItem"/>.
/// </summary>
public class Order
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string? Comment { get; set; }

    public decimal Total { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.New;

    // Связь «один к одному»: заказу соответствует единственный адрес доставки.
    public ShippingAddress ShippingAddress { get; set; } = new();

    // Связь «один ко многим»: заказ содержит набор позиций.
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

/// <summary>
/// Перечисление возможных статусов заказа на протяжении его жизненного цикла.
/// </summary>
public enum OrderStatus
{
    New = 0,
    Processing = 1,
    Delivered = 2,
    Cancelled = 3
}
