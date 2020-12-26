using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq.Examples
{
    class Program
    {
        public static void Task1(Store store)
        {
            PrintTaskToConsole("Задание 1. Вывести товары категории “Ноутбуки”.");

            IEnumerable<Product> laptops = store.Products.FilterBy(Category.Laptops);

            PrintProductsToConsole(laptops);
        }

        public static void Task2(Store store)
        {
            PrintTaskToConsole("Задание 2. Вывести товары из категории “Ноутбуки” с ценой до 50000.");

            IEnumerable<Product> cheapLaptops = store
                .Products
                .FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(50000);

            PrintProductsToConsole(cheapLaptops);
        }

        public static void Task3(Store store)
        {
            PrintTaskToConsole("Задание 3. Вывести товары из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию.");

            IEnumerable<Product> cheapLaptopsByPrice = store
                .Products
                .FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(50000)
                .OrderByDescending(p => p.Price);

            PrintProductsToConsole(cheapLaptopsByPrice);
        }

        public static void Task4(Store store)
        {
            PrintTaskToConsole("Задание 4. Вывести товары из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию, показывать только первую страницу (по 5 товаров на странице)");

            const int pageSize = 5;

            IEnumerable<Product> cheapLaptopsByPriceFirstPage = store.Products
                .FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(50000)
                .OrderBy(p => p.Price)
                .Take(pageSize);

            PrintProductsToConsole(cheapLaptopsByPriceFirstPage);
        }

        public static void Task5(Store store)
        {
            PrintTaskToConsole("Задание 5. Вывести вторую страницу товаров (по 5 товаров на странице) из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию.");

            int pageNumber = 1;
            const int pageSize = 5;

            IEnumerable<Product> cheapLaptopsByPriceSecondPage = store.Products.FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(50000)
                .OrderBy(p => p.Price) // показать, что порядок важен, сортировка должна быть до Skip/Take
                .Skip(pageNumber * pageSize)
                .Take(pageSize);

            PrintProductsToConsole(cheapLaptopsByPriceSecondPage);
        }

        public static void Task6(Store store)
        {
            PrintTaskToConsole("Задание 6. Вывести список всех категорий товаров в алфавитном порядке и количество товаров в них.");

            var categories = store.Products
                .GroupBy(p => p.Category)
                .Select(group => new {CategoryName = group.Key, ProductCount = group.Count()})
                .OrderBy(cat => cat.CategoryName);

            PrintCategoryList();

            //Локальная функция. Бывает полезной. Смотри Task9!
            void PrintCategoryList()
            {
                StringBuilder output = new StringBuilder();
                output.AppendLine("Количество товаров по категориям:");

                foreach (var category in categories)
                {
                    output.AppendLine($"{category.CategoryName}: {category.ProductCount} товаров.");
                }

                Console.WriteLine(output.ToString());
            }
        }

        public static void Task7(Store store)
        {
            PrintTaskToConsole("Задание 7. Вывести среднюю оценку пользователей для товара с названием “Lenovo Thinkpad”.");

            // В этом задании для наглядности раскомментировать Console.WriteLine(...) в product.Name, так будет видно, что:
            // 1. Single проходит всю коллекцию даже когда уже нашел нужный элемент, а First - останавливается.
            // 2. Выражение внутри Where будет выполняться заново для каждого отзыва.
            // Неправильный подход:
            // Нельзя помещать store.Products.Single(...) внутрь Where, иначе будут повторные обходы коллекции store.Products для каждого отзыва
            double avgRating1 = store.Reviews
                .Where(r => r.ProductId == store.Products.Single(p => p.Name == "Lenovo Thinkpad").Id)
                .Average(r => r.Rating);

            //Console.WriteLine($"Средняя оценка: {avgRating1}."); Console.WriteLine();
            Console.WriteLine($"Средняя оценка: {avgRating1}.{Environment.NewLine}");

            // Правильный подход:
            Product thinkpad =
                store.Products.First(p =>
                    p.Name == "Lenovo Thinkpad"); // Single, SingleOrDefault, First, FirstOrDefault
            double avgRating2 = store.Reviews.Where(r => r.ProductId == thinkpad.Id).Average(r => r.Rating);

            Console.WriteLine($"Средняя оценка: {avgRating2}.{Environment.NewLine}");
        }

        public static void Task8(Store store)
        {
            PrintTaskToConsole("Задание 8. Вывести среднюю оценку пользователей для товара с названием “Asus Zenbook”, у которого пока нет ни одного отзыва.");

            Product asus = store
                .Products
                .Single(p => p.Name == "Asus Zenbook");
            // SingleOrDefault, First, FirstOrDefault

            // Неправильный подход (упадет):
            // double avgRating3 = store.Reviews.Where(r => r.ProductId == asus.Id).Average(r => r.Rating);

            // Правильный подход (значение по умолчанию для пустой коллекции):
            int defaultRating = 3;

            double avgRating3 = store
                .Reviews
                .Where(r => r.ProductId == asus.Id)
                .Select(r => r.Rating)
                .DefaultIfEmpty(defaultRating)
                .Average();

            Console.WriteLine($"Средняя оценка: {avgRating3}.{Environment.NewLine}");
        }

        public static void Task9(Store store)
        {
            PrintTaskToConsole("Задание 9. Вывести список всех товаров, отсортированный по убыванию средней оценки (для товаров без оценки используется начальная оценка 3.0). " +
                               "Товары с одинаковой оценкой сортировать в алфавитном порядке по названию товара. Для каждого товара показывать количество отзывов.");
            
            int defaultRating = 3;

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

            PrintProductRatings();

            // Правильный подход
            var productRatings2 = store
                .Products
                .GroupJoin(store.Reviews,
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

            PrintProductRatings();

            void PrintProductRatings()
            {
                Console.WriteLine("Оценки товаров:");
                foreach (var rating in productRatings)
                {
                    Console.WriteLine($"{rating.ProductName}: {rating.Rating} ({rating.ReviewCount} отзывов).");
                }

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var store = new Store();
            PrintProductsToConsole(store.Products);

            Task1(store);
            Task2(store);
            Task3(store);
            Task4(store);
            Task5(store);
            Task6(store);
            Task7(store);
            Task8(store);
            Task9(store);

            Console.ReadLine();
        }

        private static void PrintProductsToConsole(IEnumerable<Product> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
        }

        private static void PrintTaskToConsole(string taskDescriprion)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(taskDescriprion);
            Console.ResetColor();   
        }
    }
}