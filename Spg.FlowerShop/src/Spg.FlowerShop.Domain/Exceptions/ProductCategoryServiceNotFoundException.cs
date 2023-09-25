using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class ProductCategoryServiceNotFoundException : Exception
    {
        public ProductCategoryServiceNotFoundException()
            : base()
        { }

        public ProductCategoryServiceNotFoundException(string message)
            : base(message)
        { }

        public ProductCategoryServiceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
