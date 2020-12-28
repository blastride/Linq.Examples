using System.Collections.Generic;

namespace Linq.Examples
{
    public enum Category
    {
        Laptops = 0,
        SmartPhones = 1
    }

    public class Store
    {
        public readonly ProductCollection Products;

        public readonly List<Review> Reviews = new()
        {
            new() {ProductId = 2, Rating = 5},
            new() {ProductId = 2, Rating = 4},
            new() {ProductId = 2, Rating = 5},
            new() {ProductId = 2, Rating = 3},
            new() {ProductId = 2, Rating = 4},
            new() {ProductId = 10, Rating = 1},
        };

        public Store()
        {
            Product[] products = {
                new() {Id = 1, Name = "Lenovo Yoga", Category = Category.Laptops, Price = 44000},
                new() {Id = 2, Name = "Lenovo Thinkpad", Category = Category.Laptops, Price = 140000},
                new() {Id = 3, Name = "Samsung A51", Category = Category.SmartPhones, Price = 25000},
                new() {Id = 4, Name = "Acer Aspire", Category = Category.Laptops, Price = 30000},
                new() {Id = 5, Name = "Xiaomi Redmibook", Category = Category.Laptops, Price = 47000},
                new() {Id = 6, Name = "Huawei Matebook D", Category = Category.Laptops, Price = 45000},
                new() {Id = 7, Name = "Asus Zenbook", Category = Category.Laptops, Price = 50000},
                new() {Id = 8, Name = "Apple Macbook Pro", Category = Category.Laptops, Price = 250000},
                new() {Id = 9, Name = "Apple iPhone 12", Category = Category.SmartPhones, Price = 70000},
                new() {Id = 10, Name = "HP ProBook", Category = Category.Laptops, Price = 35000},
            };
            
            Products = new ProductCollection(products, 0);
        }
    }
}