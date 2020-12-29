using System;

namespace Linq.Examples
{
    public class Product: IEquatable<Product>
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

        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _name == other._name && Id == other.Id && Category == other.Category && Price == other.Price;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Product) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, Id, (int) Category, Price);
        }
    }
}