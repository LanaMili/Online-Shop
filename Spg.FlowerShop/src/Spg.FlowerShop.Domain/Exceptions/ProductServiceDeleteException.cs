using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class ProductServiceDeleteException : Exception
    {
        public ProductServiceDeleteException()
    : base()
        { }

        public ProductServiceDeleteException(string message)
            : base(message)
        { }

        public ProductServiceDeleteException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
