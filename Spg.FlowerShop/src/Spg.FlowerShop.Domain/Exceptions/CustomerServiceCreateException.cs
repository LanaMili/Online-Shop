using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class CustomerServiceCreateException : Exception
    {
        public CustomerServiceCreateException()
            : base()
        { }

        public CustomerServiceCreateException(string message)
            : base(message)
        { }

        public CustomerServiceCreateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
