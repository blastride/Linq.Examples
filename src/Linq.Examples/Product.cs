using System;

namespace Linq.Examples
{
    public class Product
    {
        public long Id { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                // Console.WriteLine($"Прочитано свойство {nameof(Name)} объекта класса {nameof(Product)} с {nameof(Id)}={Id}");
                return _name;
            }
            set => _name = value; }

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Category}: {Name} {Price} руб. (Id={Id})";
        }
    }
}