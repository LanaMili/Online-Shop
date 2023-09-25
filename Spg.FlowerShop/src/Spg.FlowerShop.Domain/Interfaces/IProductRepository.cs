using Spg.FlowerShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Interfaces
{
    public interface IProductRepository
    {
        void Create(Product newEntity);
        void Delete(Product exEntity);
        void Update(Product newEntity);
       
    }
}
