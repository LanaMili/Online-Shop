using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class ProductRepositoryUpdateException : Exception
    {
        public ProductRepositoryUpdateException()
            : base()
        { }

        public ProductRepositoryUpdateException(string message)
            : base(message)
        { }

        public ProductRepositoryUpdateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
