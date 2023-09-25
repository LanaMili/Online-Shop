using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Exceptions
{
    public class GenericRepositoryDeleteException : Exception
    {
        public GenericRepositoryDeleteException()
            : base()
        { }

        public GenericRepositoryDeleteException(string message)
            : base(message)
        { }

        public GenericRepositoryDeleteException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
