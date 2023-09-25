using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class ProductRepositoryDeleteException : Exception
    {
        public ProductRepositoryDeleteException()
            : base()
        { }

        public ProductRepositoryDeleteException(string message)
            : base(message)
        { }

        public ProductRepositoryDeleteException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
