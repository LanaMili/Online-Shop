using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Application.ProductCategories
{
    public class ProductCategoryService : IReadOnlyGenericProductCategoryService
    {
        private readonly IReadOnlyRepositoryGeneric<ProductCategory> _readOnlyProductCategoryRepository;

        public ProductCategoryService(
            IReadOnlyRepositoryGeneric<ProductCategory> readOnlyProductCategoryRepository
            )
        {
            _readOnlyProductCategoryRepository = readOnlyProductCategoryRepository;
        }

        public IQueryable<ProductCategory> GetAll()
        {
            IQueryable<ProductCategory> productCategory = _readOnlyProductCategoryRepository.GetAll();
            return productCategory;
        }

        public ProductCategory? GetByPK<Guid>(Guid pk)
        {
            ProductCategory? prCat= _readOnlyProductCategoryRepository.GetByPK<Guid>(pk);
            return prCat;
        }
    }
}
