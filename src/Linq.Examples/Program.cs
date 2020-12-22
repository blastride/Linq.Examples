using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store();

            ShowProducts(store.Products);
            
            // 1
            IEnumerable<Product> laptops = store.Products.Where(p => p.Category == "Ноутбуки");
            ShowProducts(laptops);

            // 2
            IEnumerable<Product> cheapLaptops = laptops.Where(p => p.Price <= 50000);
            ShowProducts(cheapLaptops);

            // 3
            IEnumerable<Product> cheapLaptopsByPrice = cheapLaptops.OrderByDescending(p => p.Price);
            ShowProducts(cheapLaptopsByPrice);

            // 4
            const int pageSize = 5;
            IEnumerable<Product> cheapLaptopsByPriceFirstPage = cheapLaptops
                .OrderByDescending(p => p.Price)
                .Take(pageSize);
            ShowProducts(cheapLaptopsByPriceFirstPage);

            // 5
            int pageNumber = 2;
            IEnumerable<Product> cheapLaptopsByPriceSecondPage = cheapLaptops
                .OrderByDescending(p => p.Price) // показать, что порядок важен, сортировка должна быть до Skip/Take
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            ShowProducts(cheapLaptopsByPriceSecondPage);

            // 6
            var productRatingsCount = store.Products
                .Select(p => new
                {
                    ProductName = p.Name,
                    RatingCount = store.Reviews.Count(r => r.ProductId == p.Id)
                })
                .OrderBy(pr => pr.ProductName);

            Console.WriteLine("Product reviews count");
            foreach (var rating in productRatingsCount)
            {
                Console.WriteLine($"{rating.ProductName}: {rating.RatingCount} отзывов");
            }
            Console.WriteLine();

            // 7
            var productRatings = store.Reviews
                .GroupBy(r => r.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    Rating = group.Average(r => r.Rating)
                });

            Console.WriteLine("Product ratings");
            foreach (var rating in productRatings)
            {
                Console.WriteLine($"ProductId={rating.ProductId}: {rating.Rating}");
            }
            Console.WriteLine();

            var productRatingsWithNames = store.Reviews
                .GroupBy(r => r.ProductId)
                .Select(group => new
                {
                    ProductName = store.Products.FirstOrDefault(p => p.Id == group.Key)?.Name ?? "Удаленный товар",
                    Rating = group.Average(r => r.Rating)
                })
                .OrderBy(pr => pr.Rating);

            Console.WriteLine("Product ratings");
            foreach (var rating in productRatingsWithNames)
            {
                Console.WriteLine($"ProductId={rating.ProductName}: {rating.Rating}");
            }
            Console.WriteLine();

            // 8
            var allProductRatings = store.Products
                .Select(p => new
                {
                    ProductName = p.Name,
                    Rating = store.Reviews.Where(r => r.ProductId == p.Id)
                                          .Select(r => r.Rating)
                                          .DefaultIfEmpty(3) // показать, что без DefaultIfEmpty(0) падает
                                          .Average()
                })
                .OrderByDescending(pr => pr.Rating)
                .ThenBy(pr => pr.ProductName);

            Console.WriteLine("Product ratings");
            foreach (var rating in allProductRatings)
            {
                Console.WriteLine($"{rating.ProductName}: {rating.Rating}");
            }
            Console.WriteLine();


            Console.ReadLine();

        }
        private static void ShowProducts(IEnumerable<Product> items)
        {
            Console.WriteLine();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

    }
}