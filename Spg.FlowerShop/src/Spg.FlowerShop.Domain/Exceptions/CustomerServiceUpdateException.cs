using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class CustomerServiceUpdateException : Exception
    {
        public CustomerServiceUpdateException()
            : base()
        { }

        public CustomerServiceUpdateException(string message)
            : base(message)
        { }

        public CustomerServiceUpdateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
