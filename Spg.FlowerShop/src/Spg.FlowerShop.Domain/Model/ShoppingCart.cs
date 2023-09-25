using Spg.FlowerShop.Domain.Interfaces;

namespace Spg.FlowerShop.Domain.Model
{
    public enum Status // enumeration
    {
        Aktuell,
        InBezahlung,
        Checked,
        Abgeschlossen 
    }

    public class ShoppingCart : EntityBase, IFindableByGuid
    {
        public Guid Guid { get; private set; }
        private Status _status = new();
        public Status Status 
        { get => _status;

          private set 
            {
                if(value == Status.Abgeschlossen)
                {
                    SoldOn = DateTime.Now;
                }
                _status = value;
            }
        }

        public DateTime? SoldOn { get; private set;} // auto calculated

        public Address ShippingAddress { get; set; } = default!;
        public Address BillingAddress { get; set; } = default!;

        public int CustomerNavigationId { get; private set; }  // Customer manages SC
        public virtual Customer CustomerNavigation { get; private set; } = default!; 

        public string? PaymentMethodNavigationName { get; set; }  // init
        public virtual PaymentMethod? PaymentMethodNavigation  { get; set; } 

        private List<ShoppingCartItem> _shoppingCartItems = new();
        public virtual IReadOnlyList<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems;

        public ShoppingCart(Guid guid, Status status, Address shippingAddress, Address billingAddress)
        {
            Guid = guid;
            Status = status;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
        }
        protected ShoppingCart()
        { }

        public void AddShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            _shoppingCartItems.Add(shoppingCartItem);
        }
    }
}
