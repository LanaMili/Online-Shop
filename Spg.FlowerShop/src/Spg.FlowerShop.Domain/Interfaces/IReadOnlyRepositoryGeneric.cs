using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Interfaces
{
    public interface IReadOnlyRepositoryGeneric<TEntity>
        where TEntity : class
    {
        TEntity? GetByPK<TKey>(TKey pk);
        IQueryable<TEntity> GetAll();

        T? GetByGuid<T>(Guid guid)
            where T : class, IFindableByGuid;
    }
}
