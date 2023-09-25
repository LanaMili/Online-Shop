using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
            : base()
        { }

        public ProductNotFoundException(string message)
            : base(message)
        { }

        public ProductNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
