using GardenWalk.Components;
using GardenWalk.Components.Models;
using GardenWalk.Data;
using GardenWalk.Repositories;
using GardenWalk.Services;
using GardenWalk.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Регистрация Razor-компонентов с поддержкой интерактивного режима Blazor Server.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// Регистрация системы проверки работоспособности (healthchecks)
builder.Services.AddHealthChecks();

// Регистрация контекста базы данных EF Core (СУБД SQLite, подход CodeFirst).
// Строка подключения считывается из конфигурационного файла appsettings.json
// для исключения хранения параметров подключения в исходном коде.
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Data Source=garden.db";

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Регистрация репозиториев в контейнере зависимостей (Dependency Injection).
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Регистрация сервиса корзины с областью действия пользовательского сеанса.
builder.Services.AddScoped<CartService>();

// Регистрация валидатора формы оформления заказа (библиотека FluentValidation).
builder.Services.AddScoped<IValidator<CheckoutFormModel>, CheckoutFormValidator>();

var app = builder.Build();

// Инициализация базы данных при запуске приложения.
// При наличии применённых или ожидающих миграций выполняется их применение,
// в противном случае схема создаётся непосредственно из модели предметной области.
// После приведения схемы в актуальное состояние выполняется первичное наполнение БД.
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var db = await factory.CreateDbContextAsync();

    var pending = (await db.Database.GetPendingMigrationsAsync()).ToList();
    var applied = (await db.Database.GetAppliedMigrationsAsync()).ToList();
    if (pending.Count > 0 || applied.Count > 0)
    {
        await db.Database.MigrateAsync();
    }
    else
    {
        await db.Database.EnsureCreatedAsync();
    }

    await DatabaseInitializer.SeedAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();
// Эндпоинт для проверки доступности контейнера
app.MapHealthChecks("/health");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
