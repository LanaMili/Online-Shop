namespace Spg.FlowerShop.Domain.Model
{
    public class Price : EntityBase
    {
        public decimal Value { get; set; } = decimal.MinValue; 

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public string ProductNavigationName { get; set; } = string.Empty; 
        public virtual Product ProductNavigation { get; set; } = default!;

        internal Price(decimal value, DateTime validFrom, DateTime? validTo = null)
        {
            Value = value;
            //ProductNavigation = productNavigation;
            //ProductNavigationName = productNavigation.ProductName;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        protected Price()
        { }
    }
}