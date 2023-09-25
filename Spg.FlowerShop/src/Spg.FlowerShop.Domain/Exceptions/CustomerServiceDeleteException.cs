using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class CustomerServiceDeleteException : Exception
    {
        public CustomerServiceDeleteException()
            : base()
        { }

        public CustomerServiceDeleteException(string message)
            : base(message)
        { }

        public CustomerServiceDeleteException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
