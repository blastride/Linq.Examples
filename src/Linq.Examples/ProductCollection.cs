using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Examples
{
    public class ProductCollection : IEnumerable<Product>
    {
        private readonly Product[] _values;
        private readonly int _startingPoint;

        public ProductCollection(Product[] values, int startingPoint)
        {
            _values = values;
            _startingPoint = startingPoint;
        }

        public IEnumerator<Product> GetEnumerator()
        {
            return new ProductCollectionIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ProductCollectionIterator : IEnumerator<Product>
        {
            //Коллекция, по которой осуществляется итерация
            private readonly ProductCollection _collection;
            
            //Количество выполненных операций
            private int _position;

            public ProductCollectionIterator(ProductCollection collection)
            {
                _collection = collection ?? throw new ArgumentNullException(nameof(collection));
                _position = -1;
            }

            public bool MoveNext()
            {
                if (_position != _collection._values.Length)
                {
                    _position++;
                }

                return _position < _collection._values.Length;
            }

            public void Reset()
            {
                _position = -1;
            }

            public Product Current {
                get
                {
                    if (_position == -1 || _position == _collection._values.Length)
                    {
                        throw new InvalidOperationException();
                    }

                    int index = _position + _collection._startingPoint;
                    index = index % _collection._values.Length;
                    return _collection._values[index];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                //TBD
            }
        }
    }

    public static class ProductCollectionExtensions{
    
        public static IEnumerable<Product> FilterBy(this IEnumerable<Product> products, Category category)
        {
            return products.Where(p => p.Category == category);
        }
        
        public static IEnumerable<Product> PriceLessOrEqualThan(this IEnumerable<Product> products, decimal price)
        {
            return products.Where(p => p.Price <= price);
        }
    }
}