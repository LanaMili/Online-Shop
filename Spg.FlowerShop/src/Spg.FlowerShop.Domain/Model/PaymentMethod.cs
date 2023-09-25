namespace Spg.FlowerShop.Domain.Model
{
    public class PaymentMethod
    {
        public string Name { get; set; } = string.Empty; // pk

        private List<ShoppingCart> _shoppingCarts = new();
        public virtual IReadOnlyList<ShoppingCart> ShoppingCarts => _shoppingCarts;

        public PaymentMethod(string name)
        {
            Name = name;
        }

        protected PaymentMethod()
        { }
    }
}