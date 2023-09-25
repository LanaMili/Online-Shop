using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class GenericRepositoryUpdateException : Exception
    {
        public GenericRepositoryUpdateException()
        : base()
        { }

        public GenericRepositoryUpdateException(string message)
            : base(message)
        { }

        public GenericRepositoryUpdateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
