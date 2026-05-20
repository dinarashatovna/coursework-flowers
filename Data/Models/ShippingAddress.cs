namespace GardenWalk.Data.Models;

/// <summary>
/// Сущность «Адрес доставки». Содержит сведения о пункте доставки заказа.
/// Имеет связь «один к одному» с сущностью <see cref="Order"/>.
/// </summary>
public class ShippingAddress
{
    public int Id { get; set; }

    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string House { get; set; } = string.Empty;

    public string? Apartment { get; set; }

    public string PostalCode { get; set; } = string.Empty;

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
