namespace Linq.Examples
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }
        

        public override string ToString()
        {
            return $"{Category}: {Name} {Price} руб. (Id={Id})";
        }
    }
}