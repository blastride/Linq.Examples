using System;

namespace Linq.Examples
{
    public class Review : IEquatable<Review>
    {
        // public long Id { get; set; }

        public long ProductId { get; set; }

        public int Rating { get; set; }

        public override string ToString()
        {
            return $"Рейтинг: {Rating} продукта с ид = {ProductId}";
        }

        public bool Equals(Review other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ProductId == other.ProductId && Rating == other.Rating;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Review) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductId, Rating);
        }
    }
}