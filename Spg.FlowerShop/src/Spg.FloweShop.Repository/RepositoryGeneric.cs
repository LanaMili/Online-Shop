using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;

namespace Spg.FlowerShop.Repository
{
    public class RepositoryGeneric<TEntity> : IRepositoryGeneric<TEntity>, IReadOnlyRepositoryGeneric<TEntity>
        where TEntity : class
    {
        // ProductRepository verwendet DB, die ProductRepository bekommt
        private readonly FlowerShopContext _db;

        public RepositoryGeneric(FlowerShopContext db)
        {
            _db = db;
        }

        public TEntity? GetByPK<TKey>(TKey pk)
        {
            return _db.Set<TEntity>().Find(pk);
        }


        public T? GetByGuid<T>(Guid guid)
             where T : class, IFindableByGuid
        {
            return _db.Set<T>().SingleOrDefault(e => e.Guid == guid);
        }

        public T? GetByMail<T>(string email)
            where T : class, IFindableByMail
        {
            return _db.Set<T>().SingleOrDefault(e => e.Email == email);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _db.Set<TEntity>();
        }

        public void Create(TEntity newEntity)
        {
            try
            {
                DbSet<TEntity> dbSet = _db.Set<TEntity>();
                dbSet.Add(newEntity);
                _db.SaveChanges();  // => Insert
            }
            catch (DbUpdateException ex)
            {
                throw new GenericRepositoryCreateException("Create nicht möglich", ex);
            }
        }

        public void Delete(TEntity exEntity)
        {

            if (exEntity == null)
            {
                throw new ArgumentException(nameof(exEntity));
            }

            try
            {
                DbSet<TEntity> dbSet = _db.Set<TEntity>();
                dbSet.Remove(exEntity);
                _db.SaveChanges();  // => Deleted
            }
            catch (DbUpdateException ex)
            {
                throw new GenericRepositoryDeleteException("Delete ist nicht moeglich", ex);
            }
        }

        
        public void Update(TEntity entityToBeUpdated)
        {
            if (entityToBeUpdated == null)
            {
                throw new ArgumentException(nameof(entityToBeUpdated));
            }

            try
            {
                DbSet<TEntity> dbSet = _db.Set<TEntity>();
                dbSet.Update(entityToBeUpdated);
                _db.SaveChanges();  // => Update
            }
            catch (DbUpdateException ex)
            {
                throw new GenericRepositoryUpdateException("Update nicht möglich", ex);
            }
        }
        

    }
}
