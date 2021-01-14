using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq.Examples
{
    class Program
    {
        public static void Exercise1(Store store)
        {
            PrintExerciseTitle("Задание 1. Вывести товары категории “Ноутбуки”.");

            IEnumerable<Product> laptops = store.Products.FilterBy(Category.Laptops);
            //IEnumerable<Product> laptops = store.Products.Where(p => p.Category == Category.Laptops);;

            IEnumerable<Product> products = from product in store.Products
                where product.Category == Category.Laptops
                select product;

            Console.WriteLine("Точечная нотация.");
            PrintToConsole(laptops);

            Console.WriteLine("SQL нотация.");
            PrintToConsole(products);
        }

        public static void Exercise2(Store store)
        {
            PrintExerciseTitle("Задание 2. Вывести товары из категории “Ноутбуки” с ценой до 50000.");

            const decimal maxPrice = 50000;

            IEnumerable<Product> cheapLaptops = store
                .Products
                .FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(maxPrice);

            IEnumerable<Product> cheapLaptops2 = from product in store.Products
                where product.Category == Category.Laptops && product.Price <= maxPrice
                select product;

            Console.WriteLine("Точечная нотация.");
            PrintToConsole(cheapLaptops);

            Console.WriteLine("SQL нотация.");
            PrintToConsole(cheapLaptops2);

            // Альтернативный вариант на множествах (медленнее)
            IEnumerable<Product> cheapProducts = store
                .Products
                .PriceLessOrEqualThan(maxPrice);

            IEnumerable<Product> laptops = store
                .Products
                .FilterBy(Category.Laptops);

            IEnumerable<Product> cheapLaptops3 = cheapProducts.Intersect(laptops);

            Console.WriteLine("Пересечение множеств.");
            PrintToConsole(cheapLaptops3);
        }

        public static void Exercise3(Store store)
        {
            PrintExerciseTitle(
                "Задание 3. Вывести товары из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию.");

            const decimal maxPrice = 50000;

            IEnumerable<Product> cheapLaptopsByPrice = store
                .Products
                .FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(maxPrice)
                .OrderByDescending(p => p.Price);

            IEnumerable<Product> laptops = from product in store.Products
                where product.Category == Category.Laptops && product.Price <= maxPrice
                orderby product.Price descending
                select product;

            Console.WriteLine("Точечная нотация.");
            PrintToConsole(cheapLaptopsByPrice);

            Console.WriteLine("SQL нотация.");
            PrintToConsole(laptops);
        }

        public static void Exercise4(Store store)
        {
            PrintExerciseTitle("Задание 4. Вывести товары из категории “Ноутбуки” с ценой до 50000, " +
                               "отсортировать по цене по возрастанию, показывать только первую страницу (по 5 товаров на странице)");

            const int pageSize = 5;
            const decimal maxPrice = 50000;

            IEnumerable<Product> cheapLaptopsByPriceFirstPage = store.Products
                .FilterBy(Category.Laptops)
                .Where(p => p.Price <= maxPrice)
                .OrderBy(p => p.Price)
                .Take(pageSize);

            IEnumerable<Product> laptops = (from product in store.Products
                where product.Category == Category.Laptops && product.Price <= maxPrice
                orderby product.Price
                select product).Take(pageSize);

            Console.WriteLine("Точечная нотация.");
            PrintToConsole(cheapLaptopsByPriceFirstPage);

            Console.WriteLine("SQL нотация.");
            PrintToConsole(laptops);
        }

        public static void Exercise5(Store store)
        {
            PrintExerciseTitle("Задание 5. Вывести вторую страницу товаров (по 5 товаров на странице) " +
                               "из категории “Ноутбуки” с ценой до 50000, отсортировать по цене по возрастанию.");

            int pageNumber = 2;
            const int pageSize = 5;
            const decimal maxPrice = 50000;

            IEnumerable<Product> cheapLaptopsByPriceSecondPage = store.Products.FilterBy(Category.Laptops)
                .PriceLessOrEqualThan(maxPrice)
                .OrderBy(p => p.Price) // показать, что порядок важен, сортировка должна быть до Skip/Take
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var cheapLaptopsByPriceSecondPage2 = (from product in store.Products
                    where product.Category == Category.Laptops && product.Price <= maxPrice
                    orderby product.Price
                    select product)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            Console.WriteLine("Точечная нотация.");
            PrintToConsole(cheapLaptopsByPriceSecondPage);

            Console.WriteLine("SQL нотация.");
            PrintToConsole(cheapLaptopsByPriceSecondPage2);
        }

        public static void Exercise6(Store store)
        {
            PrintExerciseTitle("Задание 6. Вывести список всех категорий товаров в алфавитном порядке.");

            var categories = store.Products.Select(p => p.Category).Distinct().OrderBy(cat => cat);

            var categories2 = (from product in store.Products
                              select product.Category
                              into cat
                              orderby cat
                              select cat)
                              .Distinct();


            Console.WriteLine("Точечная нотация.");
            PrintCategoryList(categories);

            Console.WriteLine("SQL нотация.");
            PrintCategoryList(categories2);

            //Локальная функция
            void PrintCategoryList(dynamic categoryCollection)
            {
                Console.WriteLine("Категории товаров:");

                PrintToConsole(categoryCollection);
            }
        }

        public static void Exercise7(Store store)
        {
            PrintExerciseTitle("Задание 7. Вывести список всех категорий товаров в алфавитном порядке и количество товаров в них.");

            var categories = store.Products
                .GroupBy(p => p.Category)
                .Select(group => new {CategoryName = group.Key, ProductCount = group.Count()})
                .OrderBy(cat => cat.CategoryName);

            var categories2 = from product in store.Products
                group product by product.Category
                into grouped
                select new {CategoryName = grouped.Key, ProductCount = grouped.Count()}
                into result
                orderby result.CategoryName
                select result;


            Console.WriteLine("Точечная нотация.");
            PrintCategoryList(categories);

            Console.WriteLine("SQL нотация.");
            PrintCategoryList(categories2);

            //Локальная функция
            void PrintCategoryList(dynamic categoryCollection)
            {
                StringBuilder output = new StringBuilder();
                output.AppendLine("Количество товаров по категориям:");

                foreach (var category in categoryCollection)
                {
                    output.AppendLine($"{category.CategoryName}: {category.ProductCount} товаров.");
                }

                Console.WriteLine(output.ToString());
            }
        }

        public static void Exercise8(Store store)
        {
            PrintExerciseTitle("Задание 8. Вывести среднюю оценку пользователей для товара с названием “Lenovo Thinkpad”.");

            // В этом задании для наглядности раскомментировать Console.WriteLine(...) в product.Name, так будет видно, что:
            // 1. Single проходит всю коллекцию даже когда уже нашел нужный элемент, а First - останавливается.
            // 2. Выражение внутри Where будет выполняться заново для каждого отзыва.
            // Неправильный подход:
            // Нельзя помещать store.Products.Single(...) внутрь Where, иначе будут повторные обходы коллекции store.Products для каждого отзыва
            double avgRating1 = store.Reviews
                .Where(r => r.ProductId == store.Products.Single(p => p.Name == "Lenovo Thinkpad").Id)
                .Average(r => r.Rating);

            Console.WriteLine($"Средняя оценка: {avgRating1}.{Environment.NewLine}");

            // Правильный подход:
            Product thinkpad = store
                .Products
                .First(p => p.Name == "Lenovo Thinkpad"); // Single, SingleOrDefault, First, FirstOrDefault

            double avgRating2 = store.Reviews.Where(r => r.ProductId == thinkpad.Id).Average(r => r.Rating);

            Console.WriteLine($"Средняя оценка: {avgRating2}.{Environment.NewLine}");
        }

        public static void Exercise9(Store store)
        {
            PrintExerciseTitle("Задание 9. Вывести среднюю оценку пользователей для товара с названием “Asus Zenbook”, у которого пока нет ни одного отзыва.");

            Product asus = store
                .Products
                .Single(p => p.Name == "Asus Zenbook"); // SingleOrDefault, First, FirstOrDefault

            // Неправильный подход (упадет):
            // double avgRating3 = store.Reviews.Where(r => r.ProductId == asus.Id).Average(r => r.Rating);

            // Правильный подход (значение по умолчанию для пустой коллекции):
            const int defaultRating = 3;

            double avgRating3 = store
                .Reviews
                .Where(r => r.ProductId == asus.Id)
                .Select(r => r.Rating)
                .DefaultIfEmpty(defaultRating)
                .Average();

            Console.WriteLine($"Средняя оценка: {avgRating3}.{Environment.NewLine}");
        }

        public static void Exercise10(Store store)
        {
            PrintExerciseTitle("Задание 10. Вывести список всех товаров, отсортированный по убыванию средней оценки " +
                               "(для товаров без оценки используется начальная оценка 3.0). Товары с одинаковой оценкой сортировать в алфавитном порядке " +
                               "по названию товара. Для каждого товара показывать количество отзывов.");

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

            Console.WriteLine("Точечная нотация.");
            PrintProductRatings(productRatings);

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

            Console.WriteLine("Точечная нотация.");
            PrintProductRatings(productRatings2);

            var ratings = from product in store.Products
                join review in store.Reviews
                    on product.Id equals review.ProductId
                    into reviewCollection
                select new
                {
                    ProductName = product.Name,
                    ReviewCount = reviewCollection.Count(),
                    Rating = reviewCollection.Select(r => r.Rating).DefaultIfEmpty(defaultRating).Average()
                }
                into pr
                orderby pr.Rating descending, pr.ProductName ascending
                select pr;

            Console.WriteLine("SQL нотация.");
            PrintProductRatings(ratings);

            void PrintProductRatings(dynamic ratingsToShow)
            {
                Console.WriteLine("Оценки товаров:");
                foreach (var rating in ratingsToShow)
                {
                    Console.WriteLine($"{rating.ProductName}: {rating.Rating} ({rating.ReviewCount} отзывов).");
                }

                Console.WriteLine();
            }
        }

        public static void Exercise11(Store store)
        {
            PrintExerciseTitle("Задание 11. Вывести список неповторяющихся оценок (уникальных пар оценка - ID товара).");

            IEnumerable<Review> reviews = store.Reviews.Distinct();
            PrintToConsole(reviews);
        }

        static void Main(string[] args)
        {
            Store store = new Store();

            PrintToConsole(store.Products);

            Exercise1(store);
            Exercise2(store);
            Exercise3(store);
            Exercise4(store);
            Exercise5(store);
            Exercise6(store);
            Exercise7(store);
            Exercise8(store);
            Exercise9(store);
            Exercise10(store);
            Exercise11(store);

            Console.ReadLine();
        }

        private static void PrintToConsole<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
        }

        private static void PrintExerciseTitle(string ExerciseDescriprion)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(ExerciseDescriprion);
            Console.ResetColor();
        }
    }
}