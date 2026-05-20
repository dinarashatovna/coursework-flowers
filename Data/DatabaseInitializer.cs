using GardenWalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GardenWalk.Data;

/// <summary>
/// Класс выполняет первичное наполнение базы данных эталонными данными
/// каталога товаров. Метод вызывается при запуске приложения и осуществляет
/// добавление записей только в случае, если таблица категорий не содержит данных
/// </summary>
public static class DatabaseInitializer
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Categories.AnyAsync())
        {
            return;
        }

        var tags = new[]
        {
            new Tag { Name = "нежный" },     // 0
            new Tag { Name = "яркий" },      // 1
            new Tag { Name = "весна" },      // 2
            new Tag { Name = "лето" },       // 3
            new Tag { Name = "монобукет" },  // 4
            new Tag { Name = "полевой" },    // 5
            new Tag { Name = "романтика" },  // 6
            new Tag { Name = "праздник" }    // 7
        };
        db.Tags.AddRange(tags);

        var roses = new Category
        {
            Name = "Розы",
            Slug = "rozy",
            IconEmoji = "🌹",
            Description = "Классические монобукеты и сборные композиции с розами разных сортов."
        };
        var peonies = new Category
        {
            Name = "Пионы",
            Slug = "piony",
            IconEmoji = "🌸",
            Description = "Объёмные пышные пионы — сезонная нежность."
        };
        var meadow = new Category
        {
            Name = "Полевые букеты",
            Slug = "polevye",
            IconEmoji = "🌾",
            Description = "Букеты с летним настроением — луговые травы, ромашки, лаванда."
        };
        var tulips = new Category
        {
            Name = "Тюльпаны",
            Slug = "tulpany",
            IconEmoji = "🌷",
            Description = "Весенние тюльпаны: монобукеты и нежные сочетания пастельных оттенков."
        };
        var compositions = new Category
        {
            Name = "Композиции в коробках",
            Slug = "kompozitsii",
            IconEmoji = "🎁",
            Description = "Готовые композиции в коробках — удобный подарок без вазы."
        };

        db.Categories.AddRange(roses, peonies, meadow, tulips, compositions);

        var products = new[]
        {
            new Product
            {
                Name = "Персиковая радость",
                Slug = "persikovaya-radost",
                ShortDescription = "Букет из 25 пионовидных роз персикового оттенка.",
                Description = "Букет «Персиковая радость» — это пионовидные розы сорта Juliet в нежной упаковке из крафт-бумаги и атласной ленты. Подходит для свидания, годовщины или просто «чтобы порадовать».",
                Price = 4900m,
                Stock = 12,
                IsFeatured = true,
                MainImage = "/images/products/persikovaya-radost.png",
                Category = roses,
                Tags = new List<Tag> { tags[0], tags[4] } // нежный, монобукет
            },
            new Product
            {
                Name = "Карамельный сад",
                Slug = "karamelnyy-sad",
                ShortDescription = "Сборный букет: розы, эустома, лизиантус, эвкалипт.",
                Description = "Тёплая палитра карамели и пыльной розы: премиум-розы, эустома, лизиантус и эвкалипт. Букет, который будет долго радовать.",
                Price = 5600m,
                Stock = 8,
                IsFeatured = true,
                MainImage = "/images/products/karamelnyy-sad.png",
                Category = roses,
                Tags = new List<Tag> { tags[0], tags[3] } // нежный, лето
            },
            new Product
            {
                Name = "Облако пионов",
                Slug = "oblako-pionov",
                ShortDescription = "15 светло-розовых пионов в дизайнерской упаковке.",
                Description = "Сезонный фаворит: распускающиеся пионы, которые меняют цвет. Отличный вариант для любого возраста.",
                Price = 3900m,
                Stock = 5,
                IsFeatured = true,
                MainImage = "/images/products/oblako-pionov.png",
                Category = peonies,
                Tags = new List<Tag> { tags[0], tags[2], tags[4] } // нежный, весна, монобукет
            },
            new Product
            {
                Name = "Пионы в саду",
                Slug = "piony-v-sadu",
                ShortDescription = "Пионы малинового оттенка с зеленью.",
                Description = "Яркое настроение лета: пионы насыщенного малинового цвета, дополненные эвкалиптом.",
                Price = 7200m,
                Stock = 6,
                MainImage = "/images/products/piony-v-sadu.png",
                Category = peonies,
                Tags = new List<Tag> { tags[1], tags[3] } // яркий, лето
            },
            new Product
            {
                Name = "Лавандовое поле",
                Slug = "lavandovoe-pole",
                ShortDescription = "Лаванда, лагурус и ромашки в крафт-обёртке.",
                Description = "Полевой букет с ароматом юга Франции: свежая лаванда, мягкий лагурус и белые ромашки. Лёгкий, воздушный, романтичный.",
                Price = 3200m,
                Stock = 15,
                IsFeatured = true,
                MainImage = "/images/products/lavandovoe-pole.png",
                Category = meadow,
                Tags = new List<Tag> { tags[5], tags[3] } // полевой, лето
            },
            new Product
            {
                Name = "Лесная сказка",
                Slug = "lesnaya-skazka",
                ShortDescription = "Ромашки, васильки и сухоцветы.",
                Description = "Букет для тех, у кого ностальгия по лету в деревне: ромашки, васильки, ковыль и сухоцветы. Хорошо смотрится в керамической вазе.",
                Price = 2800m,
                Stock = 20,
                MainImage = "/images/products/lesnaya-skazka.png",
                Category = meadow,
                Tags = new List<Tag> { tags[5], tags[3] } // полевой, лето
            },
            new Product
            {
                Name = "Нежные тюльпаны",
                Slug = "nezhnye-tulpany",
                ShortDescription = "51 тюльпан пастельной палитры.",
                Description = "Тюльпаны в нежно-розовых и кремовых тонах.",
                Price = 5400m,
                Stock = 10,
                IsFeatured = true,
                MainImage = "/images/products/nezhnye-tulpany.png",
                Category = tulips,
                Tags = new List<Tag> { tags[0], tags[2], tags[4] } // нежный, весна, монобукет
            },
            new Product
            {
                Name = "Солнечный март",
                Slug = "solnechnyy-mart",
                ShortDescription = "35 жёлтых тюльпанов с мимозой.",
                Description = "Самый «весенний» букет: жёлтые тюльпаны и душистая мимоза — настоящее весеннее настроение в одном букете.",
                Price = 4100m,
                Stock = 9,
                MainImage = "/images/products/solnechnyy-mart.png",
                Category = tulips,
                Tags = new List<Tag> { tags[1], tags[2] } // яркий, весна
            },
            new Product
            {
                Name = "Розовое суфле",
                Slug = "rozovoe-sufle",
                ShortDescription = "Композиция в коробке.",
                Description = "Шляпная коробка с пионовидными розами, гвоздиками и эвкалиптом. Простоит в коробке до 10 дней.",
                Price = 4500m,
                Stock = 7,
                MainImage = "/images/products/rozovoe-sufle.png",
                Category = compositions,
                Tags = new List<Tag> { tags[0], tags[7] } // нежный, праздник
            },
            new Product
            {
                Name = "Сад в коробке",
                Slug = "sad-v-korobke",
                ShortDescription = "Микс из роз, эустомы и кустовой хризантемы.",
                Description = "Богатая композиция в круглой коробке: розы, эустома, кустовая хризантема. Прекрасный подарок коллегам.",
                Price = 5300m,
                Stock = 11,
                MainImage = "/images/products/sad-v-korobke.png",
                Category = compositions,
                Tags = new List<Tag> { tags[0], tags[3] } // нежный, лето
            },
            new Product
            {
                Name = "Пыльная роза",
                Slug = "pylnaya-roza",
                ShortDescription = "Букет в пыльных тонах.",
                Description = "По-настоящему душевный букет: пыльно-розовые розы Quicksand, лизиантус, гипсофила, эвкалипт и атласные ленты.",
                Price = 8200m,
                Stock = 4,
                MainImage = "/images/products/pylnaya-roza.png",
                Category = roses,
                Tags = new List<Tag> { tags[0], tags[6] } // нежный, романтика
            },
            new Product
            {
                Name = "Полевая нежность",
                Slug = "polevaya-nezhnost",
                ShortDescription = "Хризантемы, статица, лагурус.",
                Description = "Лёгкий букет в стиле прованс: кустовые хризантемы, статица, лагурус. Долго стоит в воде.",
                Price = 3400m,
                Stock = 13,
                MainImage = "/images/products/polevaya-nezhnost.png",
                Category = meadow,
                Tags = new List<Tag> { tags[0], tags[5] } // нежный, полевой
            }
        };

        // Заполнение коллекции дополнительных изображений товара
        // (отношение «один ко многим»: Product — ProductImage).
        // Для каждого товара добавляется одна стилизованная иллюстрация
        // в формате SVG, дополняющая основное изображение.
        foreach (var p in products)
        {
            p.Gallery.Add(new ProductImage
            {
                Url = $"/images/products/{p.Slug}.svg",
                Alt = $"{p.Name} — иллюстрация",
                Order = 1
            });
        }

        db.Products.AddRange(products);

        await db.SaveChangesAsync();
    }
}
