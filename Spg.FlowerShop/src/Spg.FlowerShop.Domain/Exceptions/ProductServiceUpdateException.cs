using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class ProductServiceUpdateException : Exception
    {
        public ProductServiceUpdateException()
            : base()
        { }

        public ProductServiceUpdateException(string message)
            : base(message)
        { }

        public ProductServiceUpdateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
