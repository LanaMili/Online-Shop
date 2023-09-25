using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class GenericRepositoryCreateException : Exception
    {
        public GenericRepositoryCreateException()
            : base()
        { }

        public GenericRepositoryCreateException(string message)
            : base(message)
        { }

        public GenericRepositoryCreateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
