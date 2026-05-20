namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Позиция заказа». Описывает один товар в составе заказа
/// с зафиксированной на момент покупки ценой. Имеет связь
/// «многие к одному» с сущностями <see cref="Order"/> и <see cref="Product"/>.
/// </summary>
public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string ProductNameSnapshot { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}
