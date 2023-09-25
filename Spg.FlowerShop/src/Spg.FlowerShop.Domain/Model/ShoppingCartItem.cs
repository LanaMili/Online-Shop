namespace Spg.FlowerShop.Domain.Model
{
    public class ShoppingCartItem : EntityBase
    {
        public int ItemCount { get; set; }
        public decimal Price { get; set; } 
        public string ProductNavigationName { get; set; } = string.Empty;
        public virtual Product ProductNavigation { get; set; } = default!;

        public int ShoppingCartNavigationId { get; private set; }   
        public virtual ShoppingCart ShoppingCartNavigation { get; set; } = default!;

        public ShoppingCartItem(Product productNavigation, int itemCount)
        {
            ProductNavigation = productNavigation;
            ProductNavigationName = productNavigation.ProductName;
            ItemCount = itemCount;
        }
        protected ShoppingCartItem()
        { }

    }
}
