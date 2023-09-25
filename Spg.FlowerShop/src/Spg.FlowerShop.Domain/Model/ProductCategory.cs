using Spg.FlowerShop.Domain.Interfaces;

namespace Spg.FlowerShop.Domain.Model
{
    public class ProductCategory : IFindableByGuid
    {
        public Guid Guid { get; private set; }  // pk
        public string Name { get; private set; } = string.Empty; 

        public string Description { get; set; } = string.Empty;

        private List<Product> _products = new();
        public virtual IReadOnlyList<Product> Products => _products;

        public ProductCategory(Guid guid, string name, string description)
        {
            Guid = guid;    
            Name = name;
            Description = description;
        }

        protected ProductCategory()
        { }
    }
}