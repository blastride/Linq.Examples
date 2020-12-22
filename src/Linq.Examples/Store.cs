using System.Collections.Generic;

namespace Linq.Examples
{
    public class Store
    {
        public List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Lenovo Yoga", Category = "Ноутбуки", Price = 44000 },
            new Product { Id = 2, Name = "Apple iPhone 12", Category = "Смартфоны", Price = 70000 },
            new Product { Id = 3, Name = "Samsung A51", Category = "Смартфоны", Price = 25000 },
            new Product { Id = 4, Name = "Acer Aspire", Category = "Ноутбуки", Price = 30000 },
            new Product { Id = 5, Name = "Xiaomi Redmibook", Category = "Ноутбуки", Price = 47000 },
            new Product { Id = 6, Name = "Huawei Matebook D", Category = "Ноутбуки", Price = 45000 },
            new Product { Id = 7, Name = "Asus Zenbook", Category = "Ноутбуки", Price = 50000 },
            new Product { Id = 8, Name = "Apple Macbook Pro", Category = "Ноутбуки", Price = 250000 },
            new Product { Id = 9, Name = "Lenovo Thinkpad", Category = "Ноутбуки", Price = 140000 },
            new Product { Id = 10, Name = "HP ProBook", Category = "Ноутбуки", Price = 35000 },
        };

        public List<Review> Reviews = new List<Review>
        {
            new Review { ProductId = 9, Rating = 5.0 },
            new Review { ProductId = 9, Rating = 4.0 },
            new Review { ProductId = 10, Rating = 1.0 },
        };
    }
}