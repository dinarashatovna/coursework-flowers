namespace GardenWalk.Components.Models;

/// <summary>
/// Модель формы оформления заказа. Используется для передачи данных
/// между Razor-компонентом страницы оформления и валидатором.
/// </summary>
public class CheckoutFormModel
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string? Apartment { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string? Comment { get; set; }
}
