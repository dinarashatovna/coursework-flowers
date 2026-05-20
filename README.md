# Разработка веб-системы для цветочного магазина 

Курсовой проект по дисциплине **«Кроссплатформенная среда исполнения программного обеспечения»**.  

Интернет-магазин цветов на **.NET 8 + Blazor Server + EF Core (CodeFirst, SQLite)**, полностью упакован в Docker.

---

## Публичные ссылки

| Ресурс | Ссылка |
|--------|--------|
| Репозиторий | `https://github.com/<dinarashatovna>/CourseWork_Flowers` |
| Docker Hub  | `https://hub.docker.com/r/<dinaravaleeva>/coursework-flowers` |

---

## Технологический стек

| Слой | Технология |
|------|-----------|
| Среда исполнения | .NET 8 (ASP.NET Core) |
| UI | Blazor Server (интерактивный режим, Razor Components) |
| ORM / БД | Entity Framework Core 8, SQLite, CodeFirst + Migrations |
| Валидация | FluentValidation 11 |
| Контейнеризация | Docker (multi-stage build), Docker Compose |

---

## Структура проекта

```
GardenWalk/
├── Components/                        # Razor-компоненты Blazor
│   ├── App.razor                      # Корневой документ
│   ├── Routes.razor                   # Маршрутизация
│   ├── Layout/
│   │   └── MainLayout.razor           # Шапка, навигация, подвал
│   ├── Pages/
│   │   ├── Home.razor                 # Главная (хиты, категории)
│   │   ├── Catalog.razor              # Каталог с фильтром и поиском
│   │   ├── ProductPage.razor          # Карточка товара с галереей
│   │   ├── Cart.razor                 # Корзина
│   │   ├── Checkout.razor             # Оформление заказа
│   │   ├── OrderConfirmation.razor    # Страница подтверждения
│   │   ├── About.razor                # О магазине
│   │   └── Contacts.razor             # Контакты
│   ├── Shared/
│   │   ├── ProductCard.razor          # Карточка товара (переиспользуемый компонент)
│   │   └── ConfirmModal.razor         # Модальное окно подтверждения
│   └── Models/
│       └── CheckoutFormModel.cs       # Модель формы оформления заказа
├── Data/
│   ├── AppDbContext.cs                # DbContext + Fluent API конфигурация
│   ├── DatabaseInitializer.cs         # Seed-данные при первом запуске
│   └── Models/                        # Сущности EF Core
│       ├── Category.cs
│       ├── Product.cs
│       ├── ProductImage.cs
│       ├── Tag.cs
│       ├── Order.cs
│       ├── OrderItem.cs
│       └── ShippingAddress.cs
├── Repositories/                      # Репозитории + интерфейсы
│   ├── IProductRepository.cs
│   ├── ICategoryRepository.cs
│   ├── IOrderRepository.cs
│   ├── ProductRepository.cs
│   ├── CategoryRepository.cs
│   └── OrderRepository.cs
├── Services/
│   └── CartService.cs                 # Сервис корзины (Scoped)
├── Validators/
│   └── CheckoutFormValidator.cs       # FluentValidation для формы заказа
├── wwwroot/
│   ├── css/site.css                   # Стили (пастельная палитра)
│   └── images/products/               # Изображения каталога
├── appsettings.json
├── Program.cs
├── GardenWalk.csproj
├── Dockerfile
└── docker-compose.yml
```

---

## Модель данных

```
┌─────────────┐        ┌─────────────────┐        ┌──────────────┐
│  Category   │ 1    N │    Product      │ N    N │     Tag      │
│─────────────│────────│─────────────────│────────│──────────────│
│ Id          │        │ Id              │        │ Id           │
│ Name        │        │ Name            │        │ Name         │
│ Slug        │        │ Slug            │        └──────────────┘
│ Description │        │ ShortDescription│               (ProductTags)
│ IconEmoji   │        │ Description     │
└─────────────┘        │ Price           │        ┌──────────────┐
                       │ Stock           │ 1    N │ ProductImage │
                       │ IsFeatured      │────────│──────────────│
                       │ MainImage       │        │ Id           │
                       │ CategoryId (FK) │        │ Url          │
                       └─────────────────┘        │ Alt          │
                                                  │ ProductId(FK)│
┌─────────────┐        ┌─────────────────┐        └──────────────┘
│    Order    │ 1    1 │ ShippingAddress │
│─────────────│────────│─────────────────│
│ Id          │        │ Id              │
│ CustomerName│        │ City            │
│ CustomerPhone        │ Street          │
│ CustomerEmail        │ House           │
│ Comment     │        │ Apartment       │
│ Total       │        │ PostalCode      │
│ CreatedAt   │        │ OrderId (FK)    │
└─────────────┘        └─────────────────┘
       │ 1
       │ N
┌─────────────┐
│  OrderItem  │
│─────────────│
│ Id          │
│ ProductId   │
│ Quantity    │
│ UnitPrice   │
│ ProductName │
│   Snapshot  │
│ OrderId(FK) │
└─────────────┘
```

Связи сущностей:

- **Category ↔ Product** — 1 : N (категория содержит много товаров)
- **Product ↔ ProductImage** — 1 : N (галерея изображений товара)
- **Product ↔ Tag** — N : N (через таблицу `ProductTags`)
- **Order ↔ ShippingAddress** — 1 : 1 (у каждого заказа один адрес доставки)
- **Order ↔ OrderItem** — 1 : N (заказ содержит позиции)

Конфигурация выполнена через **Fluent API** в `AppDbContext.OnModelCreating`. Доступ к данным изолирован через **репозитории** (`IProductRepository`, `ICategoryRepository`, `IOrderRepository`), зарегистрированные в `IServiceCollection` (DI) в `Program.cs`.

---

## Установка и запуск

### Требования

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Docker 20+](https://www.docker.com/) и Docker Compose 2+ (для контейнерного запуска)

### Локальный запуск (без Docker)

```bash
# 1. Восстановить зависимости
dotnet restore

# 2. Применить миграции / создать базу данных
dotnet ef database update

# 3. Запустить приложение
dotnet run
```

> При отсутствии миграций приложение автоматически создаёт схему через `EnsureCreated`
> и заполняет каталог тестовыми данными.

Приложение откроется по адресу: [http://localhost:8007](http://localhost:8007)

### Запуск в Docker

```bash
# Сборка образа
docker build -t coursework-flowers:latest .

# Запуск через Docker Compose
docker-compose up
```

Запуск в фоне:

```bash
docker-compose up -d
```

Остановка (volume с базой данных сохраняется):

```bash
docker-compose down
```

Приложение доступно по адресу: [http://localhost:8007](http://localhost:8007)

> SQLite-файл хранится в именованном томе `coursework-flowers-data` (смонтирован в `/app/data`
> внутри контейнера) — данные не теряются при перезапуске контейнера.

### Healthcheck

Проверка работоспособности реализована на трёх уровнях:

**1. ASP.NET Core (`Program.cs`)**

Встроенная подсистема `Microsoft.Extensions.Diagnostics.HealthChecks` регистрируется и подключается явным эндпоинтом:

```csharp
builder.Services.AddHealthChecks();          // регистрация сервиса
// ...
app.MapHealthChecks("/health");              // HTTP GET /health → 200 OK / 503
```

При обращении к `/health` ASP.NET Core возвращает `200 Healthy` если все зарегистрированные проверки пройдены, иначе `503 Unhealthy`.

**2. Dockerfile**

`wget` устанавливается в runtime-образе и используется как инструмент проверки:

```dockerfile
RUN apt-get update \
    && apt-get install -y --no-install-recommends wget \
    && rm -rf /var/lib/apt/lists/*

HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
    CMD wget -qO- http://localhost:8007/health >/dev/null 2>&1 || exit 1
```

**3. Docker Compose (`docker-compose.yml`)**

Параллельная проверка на уровне оркестратора (перекрывает настройки Dockerfile):

```yaml
healthcheck:
  test: ["CMD-SHELL", "wget --no-verbose --tries=1 --spider http://127.0.0.1:8007/health || exit 1"]
  interval: 30s
  timeout: 5s
  retries: 3
  start_period: 20s
```

Проверить текущий статус контейнера:

```bash
docker inspect --format='{{.State.Health.Status}}' coursework-flowers
```

Возможные статусы: `starting` → `healthy` / `unhealthy`.

### Публикация образа на Docker Hub

```bash
docker build -t <username>/coursework-flowers:latest .
docker push <username>/coursework-flowers:latest
```

---

## Функциональность

- **Главная страница** — категории товаров.
- **Каталог** — фильтрация по категории, текстовый поиск (`?category=...&q=...`).
- **Карточка товара** — галерея изображений, теги, описание, кнопка «В корзину».
- **Корзина** — изменение количества, удаление позиции, очистка с модальным подтверждением.
- **Оформление заказа** — форма с валидацией через FluentValidation (имя, телефон, email, адрес доставки).
- **Страница подтверждения** — итоги заказа со всеми деталями.
- **Адаптивная вёрстка** — оптимизирована под мобильные устройства.

---

## Соответствие требованиям курсовой работы

| Требование | Реализация |
|-----------|-----------|
| .NET 8+ | `<TargetFramework>net8.0</TargetFramework>` в `GardenWalk.csproj` |
| ASP.NET Core | Razor Components / Blazor Server |
| EF Core CodeFirst | `AppDbContext`, миграции через `dotnet ef migrations add` |
| Отношения 1:1, 1:N, N:N | `Order ↔ ShippingAddress`, `Category ↔ Product`, `Product ↔ Tag` |
| Репозитории | `Repositories/*Repository.cs` + интерфейсы |
| `DbContext` с Fluent API | `AppDbContext.OnModelCreating` |
| Dependency Injection | `Program.cs` — `IServiceCollection` |
| Blazor: формы, валидация, модальные окна | `Checkout.razor`, `CheckoutFormValidator`, `ConfirmModal.razor` |
| Multi-stage Dockerfile | `Dockerfile` (sdk → aspnet) |
| Volume для БД | `coursework-flowers-data` в `docker-compose.yml` |
| Healthcheck | `AddHealthChecks()` + `MapHealthChecks("/health")` в `Program.cs`; `HEALTHCHECK CMD wget .../health` в `Dockerfile`; дублирующая проверка в `docker-compose.yml` |
| Отсутствие hardcoded данных | строка подключения — из `appsettings.json` |
