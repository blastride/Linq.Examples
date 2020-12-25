using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Examples
{
    class Program
    {
        public static void Task1(Store store)
        {
            Console.WriteLine("Задание 1. Вывести товары категории “Ноутбуки”.");
            
            IEnumerable<Product> laptops = store.Products.FilterBy(Category.Laptops);
            ShowProducts(laptops);
        }
        
        public static void Task2(Store store)
        {
            Console.WriteLine("Задание 2. Вывести товары из категории “Ноутбуки” с ценой до 50000.");
            
            IEnumerable<Product> cheapLaptops = store.Products.FilterBy(Category.Laptops).PriceLessOrEqualThan(50000);
            ShowProducts(cheapLaptops);
        }

        public static void Task3(Store store)
        {
            Console.WriteLine("Задание 3. Вывести товары из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию.");
            
            IEnumerable<Product> cheapLaptopsByPrice = store.Products.FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(50000).OrderByDescending(p => p.Price);
            ShowProducts(cheapLaptopsByPrice);
        }

        static void Main(string[] args)
        {
            var store = new Store();
            ShowProducts(store.Products);

            Task1(store);
            Task2(store);
            Task3(store);

            Console.WriteLine("Задание 4. Вывести товары из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию, показывать только первую страницу (по 5 товаров на странице)");
            const int pageSize = 5;
            IEnumerable<Product> cheapLaptopsByPriceFirstPage = store.Products
                .FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(50000)
                .OrderBy(p => p.Price)
                .Take(pageSize);
            ShowProducts(cheapLaptopsByPriceFirstPage);

            Console.WriteLine("Задание 5.");
            int pageNumber = 2;
            IEnumerable<Product> cheapLaptopsByPriceSecondPage = store.Products.FilterBy(Category.Laptops).PriceLessOrEqualThan(50000)
                .OrderBy(p => p.Price) // показать, что порядок важен, сортировка должна быть до Skip/Take
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            ShowProducts(cheapLaptopsByPriceSecondPage);

            Console.WriteLine("Задание 6.");
            var categories = store.Products
                .GroupBy(p => p.Category)
                .Select(group => new {CategoryName = group.Key, ProductCount = group.Count()})
                .OrderBy(cat => cat.CategoryName);

            Console.WriteLine("Количество товаров по категориям:");
            foreach (var category in categories)
            {
                Console.WriteLine($"{category.CategoryName}: {category.ProductCount} товаров");
            }

            Console.WriteLine();

            Console.WriteLine("Задание 7.");
            // В этом задании для наглядности раскомментировать Console.WriteLine(...) в product.Name, так будет видно, что:
            // 1. Single проходит всю коллекцию даже когда уже нашел нужный элемент, а First - останавливается.
            // 2. Выражение внутри Where будет выполняться заново для каждого отзыва.
            // Неправильный подход:
            // Нельзя помещать store.Products.Single(...) внутрь Where, иначе будут повторные обходы коллекции store.Products для каждого отзыва
            double avgRating1 = store.Reviews
                .Where(r => r.ProductId == store.Products.Single(p => p.Name == "Lenovo Thinkpad").Id)
                .Average(r => r.Rating);
            Console.WriteLine($"Средняя оценка: {avgRating1}.");
            Console.WriteLine();

            // Правильный подход:
            Product thinkpad =
                store.Products.First(p =>
                    p.Name == "Lenovo Thinkpad"); // Single, SingleOrDefault, First, FirstOrDefault
            double avgRating2 = store.Reviews.Where(r => r.ProductId == thinkpad.Id).Average(r => r.Rating);
            Console.WriteLine($"Средняя оценка: {avgRating2}.");
            Console.WriteLine();

            Console.WriteLine("Задание 8.");
            Product asus =
                store.Products.Single(p => p.Name == "Asus Zenbook"); // SingleOrDefault, First, FirstOrDefault
            // Неправильный подход (упадет):
            // double avgRating3 = store.Reviews.Where(r => r.ProductId == asus.Id).Average(r => r.Rating);

            // Правильный подход (значение по умолчанию для пустой коллекции):
            int defaultRating = 3;
            double avgRating3 = store.Reviews.Where(r => r.ProductId == asus.Id).Select(r => r.Rating)
                .DefaultIfEmpty(defaultRating).Average();
            Console.WriteLine($"Средняя оценка: {avgRating3}.");
            Console.WriteLine();

            Console.WriteLine("Задание 9.");
            // Неправильный подход (многократный перебор коллекций):
            var productRatings = store.Products
                .Select(p => new
                {
                    ProductName = p.Name,
                    ReviewCount = store.Reviews.Count(r => r.ProductId == p.Id),
                    Rating = store.Reviews.Where(r => r.ProductId == p.Id).Select(r => r.Rating)
                        .DefaultIfEmpty(defaultRating).Average()
                })
                .OrderByDescending(pr => pr.Rating)
                .ThenBy(pr => pr.ProductName);

            Console.WriteLine("Оценки товаров:");
            foreach (var rating in productRatings)
            {
                Console.WriteLine($"{rating.ProductName}: {rating.Rating} ({rating.ReviewCount} отзывов)");
            }

            Console.WriteLine();

            // Правильный подход
            var productRatings2 = store.Products.GroupJoin(store.Reviews,
                    product => product.Id,
                    review => review.ProductId,
                    (product, reviewCollection) => new
                    {
                        ProductName = product.Name,
                        ReviewCount = reviewCollection.Count(),
                        Rating = reviewCollection.Select(r => r.Rating).DefaultIfEmpty(defaultRating).Average()
                    })
                .OrderByDescending(pr => pr.Rating)
                .ThenBy(pr => pr.ProductName);

            Console.WriteLine("Оценки товаров:");
            foreach (var rating in productRatings)
            {
                Console.WriteLine($"{rating.ProductName}: {rating.Rating} ({rating.ReviewCount} отзывов)");
            }

            Console.WriteLine();

            Console.ReadLine();
        }

        private static void ShowProducts(IEnumerable<Product> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
        }
    }
}