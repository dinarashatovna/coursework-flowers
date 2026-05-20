using GardenWalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GardenWalk.Data;

/// <summary>
/// Контекст базы данных приложения. Конфигурация сущностей предметной
/// области и их взаимосвязей выполняется с использованием Fluent API
/// в методе <see cref="OnModelCreating"/>.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<ShippingAddress> ShippingAddresses => Set<ShippingAddress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(80);
            e.Property(x => x.Slug).IsRequired().HasMaxLength(80);
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.IconEmoji).HasMaxLength(8);
        });

        // Сущность «Товар»: связь «многие к одному» с категорией и
        // связь «один ко многим» с галереей изображений.
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(120);
            e.Property(x => x.Slug).IsRequired().HasMaxLength(120);
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.ShortDescription).HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Price).HasColumnType("decimal(10,2)");
            e.Property(x => x.MainImage).HasMaxLength(260);

            e.HasOne(x => x.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Gallery)
                .WithOne(g => g.Product)
                .HasForeignKey(g => g.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Сущность «Изображение товара».
        modelBuilder.Entity<ProductImage>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Url).IsRequired().HasMaxLength(260);
            e.Property(x => x.Alt).HasMaxLength(160);
        });

        // Сущность «Тег» и связь «многие ко многим» с товаром.
        modelBuilder.Entity<Tag>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(60);
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Tags)
            .WithMany(t => t.Products)
            .UsingEntity(j => j.ToTable("ProductTags"));

        // Сущность «Заказ»: связь «один к одному» с адресом доставки
        // и связь «один ко многим» с позициями заказа.
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.CustomerName).IsRequired().HasMaxLength(120);
            e.Property(x => x.CustomerPhone).IsRequired().HasMaxLength(40);
            e.Property(x => x.CustomerEmail).IsRequired().HasMaxLength(160);
            e.Property(x => x.Comment).HasMaxLength(500);
            e.Property(x => x.Total).HasColumnType("decimal(10,2)");

            e.HasOne(x => x.ShippingAddress)
                .WithOne(a => a.Order)
                .HasForeignKey<ShippingAddress>(a => a.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ProductNameSnapshot).IsRequired().HasMaxLength(160);
            e.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
            e.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShippingAddress>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.City).IsRequired().HasMaxLength(80);
            e.Property(x => x.Street).IsRequired().HasMaxLength(120);
            e.Property(x => x.House).IsRequired().HasMaxLength(20);
            e.Property(x => x.Apartment).HasMaxLength(20);
            e.Property(x => x.PostalCode).IsRequired().HasMaxLength(20);
        });
    }
}
