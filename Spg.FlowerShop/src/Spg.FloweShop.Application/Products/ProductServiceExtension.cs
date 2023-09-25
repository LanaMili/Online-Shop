using Spg.FlowerShop.Domain.Model;
using System.Linq.Dynamic;


namespace Spg.FlowerShop.Application.Products
{
    public static class ProductServiceExtension
    {
        private static readonly Dictionary<Type, Func<IEnumerable<Product>, object, IEnumerable<Product>>> Filters = new Dictionary<Type, Func<IEnumerable<Product>, object, IEnumerable<Product>>>()
        {
            { typeof(string), (products, filter) => products.Where(p => p.ProductName.ToLower().Contains(((string)filter).ToLower())) },
            { typeof(int), (products, filter) => products.Where(p => p.CurrentPrice >= (int)filter) },
            { typeof(Guid), (products, filter) => products.Where(p => p.ProductCategoryNavigationGuid == (Guid)filter) },
            { typeof(ProductCategory), (products, filter) => products.Where(p => p.ProductCategoryNavigation == (ProductCategory)filter) },
        };

        public static IEnumerable<Product> Filter<TKey>(
            this IEnumerable<Product> products,
            TKey filter)
        {
            var filterType = typeof(TKey);

            if (Filters.ContainsKey(filterType) && filter != null)
            {
                return Filters[filterType](products, filter);
            }

            throw new ArgumentException("Unsupported filter type.");
        }

        public static IEnumerable<Product> Paging(
            this IEnumerable<Product> products,
            int size,
            int pageNumber)
        {
            if (size <= 0 || pageNumber <= 0)
            {
                throw new ArgumentException("Invalid size or page number.");
            }

            return products
                .Skip((pageNumber - 1) * size)
                .Take(size);
        }

        public static IEnumerable<Product> Sorting(
            this IEnumerable<Product> productsToBeSorted,
            string sortBy,
            bool ascending = true)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                throw new ArgumentException("SortBy property cannot be null or empty.");
            }

            string sortExpression = $"{sortBy} {(ascending ? "ASC" : "DESC")}";
            return productsToBeSorted.OrderBy(sortExpression);
        }
    }
}
