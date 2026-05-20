using FluentValidation;
using GardenWalk.Components.Models;

namespace GardenWalk.Validators;

/// <summary>
/// Валидатор формы оформления заказа, реализованный средствами библиотеки
/// FluentValidation. Проверяет корректность заполнения полей покупателя
/// и адреса доставки до сохранения заказа в базе данных.
/// </summary>
public class CheckoutFormValidator : AbstractValidator<CheckoutFormModel>
{
    public CheckoutFormValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Укажите имя получателя.")
            .MaximumLength(120);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Укажите телефон для связи.")
            .Matches(@"^[+\d][\d\s\-\(\)]{6,}$").WithMessage("Похоже, телефон указан некорректно.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Укажите email.")
            .EmailAddress().WithMessage("Email указан некорректно.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Укажите город.")
            .MaximumLength(80);

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Укажите улицу.")
            .MaximumLength(120);

        RuleFor(x => x.House)
            .NotEmpty().WithMessage("Укажите номер дома.")
            .MaximumLength(20);

        RuleFor(x => x.Apartment)
            .MaximumLength(20);

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Укажите индекс.")
            .Matches(@"^\d{5,6}$").WithMessage("Индекс должен содержать 5–6 цифр.");

        RuleFor(x => x.Comment)
            .MaximumLength(500);
    }
}
