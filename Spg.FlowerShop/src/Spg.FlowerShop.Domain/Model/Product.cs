using Spg.FlowerShop.Domain.Interfaces;

namespace Spg.FlowerShop.Domain.Model
{
    public class Product
    {
        public string ProductName { get; private set; } = string.Empty;  // pk

        public decimal CurrentPrice
        {
            get
            {
                return Prices.Single(p => !p.ValidTo.HasValue).Value;
            }
            
            set
            {
                DateTime currentDateTime = DateTime.Now;

                if (Prices.Count() > 0) // !!
                {
                    Price exCurrentPrice = Prices.Single(p => !p.ValidTo.HasValue);
                    exCurrentPrice.ValidTo = currentDateTime;
                }
                _prices.Add(new Price(value,  currentDateTime, null));
            }
        }

        public string Ean { get; set; } = string.Empty;

        public decimal AvgScore => (decimal) _reviews.Select(r => r.ReviewScore).DefaultIfEmpty(0).Average();
        public Guid ProductCategoryNavigationGuid { get; set; } 
        public virtual ProductCategory ProductCategoryNavigation { get; set; } = default!; // vk

        public string ProductImage { get; set; } = string.Empty;

        private List<Price> _prices = new();
        public virtual IReadOnlyList<Price> Prices => _prices;

        private List<Review> _reviews = new();
        public virtual IReadOnlyList<Review> Reviews => _reviews;
                
        private List<ShoppingCartItem> _shoppingCartItems = new();
        public virtual IReadOnlyList<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems; 

        public Product(string productName, decimal initialCurrentPrice, string ean, ProductCategory productCategoryNavigation, string productImage)
        {
            ProductName = productName;
            CurrentPrice = initialCurrentPrice;
            Ean = ean;
            ProductCategoryNavigation = productCategoryNavigation;
            ProductCategoryNavigationGuid = productCategoryNavigation.Guid;
            ProductImage = productImage;
        }
        protected Product()
        { }
    }
}
