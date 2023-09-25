using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Interfaces
{
    public interface IRepositoryGeneric<TEntity>
        where TEntity : class
    {
        void Create(TEntity newEntity);
        void Update(TEntity newEntity);
        void Delete(TEntity newEntity);
    }
}
