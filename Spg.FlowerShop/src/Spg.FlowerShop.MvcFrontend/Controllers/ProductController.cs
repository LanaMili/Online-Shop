using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Spg.FlowerShop.Application.Products;
using Spg.FlowerShop.Domain.DataTransferObjects;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;

namespace Spg.FlowerShop.MvcFrontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IReadOnlyProductService _readOnlyProductService;
        private readonly IAddableProductService _addableProductService;
        private readonly IReadOnlyGenericProductCategoryService _readOnlyProductCategoryService;
        private readonly IDeletableProductService _deletableProductService;
        private readonly IUpdateableProductService _updateableProductService;

        public ProductController(
            IReadOnlyProductService readOnlyProductService,
            IAddableProductService addableProductService,
            IReadOnlyGenericProductCategoryService readOnlyProductCategoryService,
            IDeletableProductService deletableProductService,
            IUpdateableProductService updateableProductService
            )
        {
            _readOnlyProductService = readOnlyProductService;
            _addableProductService = addableProductService;
            _readOnlyProductCategoryService = readOnlyProductCategoryService;
            _deletableProductService = deletableProductService;
            _updateableProductService = updateableProductService;
        }

        // Use-case 1 get
        [HttpGet()]
        public IActionResult Index()
        {
            ViewBag.Products = _readOnlyProductService.GetAll();
            ViewBag.ProductCategories = _readOnlyProductCategoryService.GetAll();

            return View();
        }

        // Use-case 1 post
        [HttpPost()]
        public IActionResult Index(string searchString, Guid searchCategory, string asc, int pageSize, int pageNumber)
        {
            ViewBag.SearchString = searchString;
            ViewBag.Asc = asc;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            if(pageSize <= 0)
            {
                pageSize = 99;
            }

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            IEnumerable<Product> products = _readOnlyProductService.GetAll();

            
            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    products = products.Filter(searchString);
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError(string.Empty, "Ungültiger Such-/Filterwert.");
                }
            }

            if (searchCategory != Guid.Empty)
            {
                try
                {
                    products = products.Filter(searchCategory);
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError(string.Empty, "Ungültiger Such-/Filterwert.");
                }
            }

            bool direction = asc == "on";

            ViewBag.Products = products.Sorting("ProductName", direction).Paging(pageSize, pageNumber);
            ViewBag.ProductCategories = _readOnlyProductCategoryService.GetAll();
            return View();
        }

        // Use-case 2 get
        [HttpGet()]
        public IActionResult Create()
        {
            ProductDto model = new ProductDto();
            ViewBag.ProductCategories = _readOnlyProductCategoryService.GetAll();   // ProductCategories sind notwendig
            return View(model);
        }

        // Use-case 2 post
        [HttpPost()]
        public IActionResult Create(ProductDto newProduct)
        {
            ProductCategory? prCat = _readOnlyProductCategoryService.GetByPK<Guid>(newProduct.ProductCategoryID);

            if (prCat == null)
            {
                throw new ProductServiceCreateException();
            }
            Product newProductDM = new Product(
                newProduct.ProductName,
                newProduct.CurrentPrice,
                newProduct.Ean,
                prCat,
                newProduct.ProductImage
                );

            if (!ModelState.IsValid)
            {
                // Postback
                ViewBag.ProductCategories = _readOnlyProductCategoryService.GetAll();
                return View(newProduct);
            }
            try
            {
                _addableProductService.Create(newProductDM);
            }
            catch(ProductServiceCreateException csCreate)
            {
                ModelState.AddModelError(string.Empty, csCreate.Message);
                ViewBag.ProductCategories = _readOnlyProductCategoryService.GetAll();
                return View(newProduct);
            }
            return RedirectToAction("Index");
        }


         // Use-case 3 
        [HttpGet()]
        public IActionResult Delete(string id)
        {
            try
            {
                Product? productToBeDeleted = _readOnlyProductService.ProductGetById(id);

                if(productToBeDeleted == null)
                {
                    ModelState.AddModelError(string.Empty, "productToBeDeleted existiert nicht");
                    return View();
                }
                
                ProductDto model = new ProductDto()
                {
                    ProductName = productToBeDeleted.ProductName,
                    CurrentPrice = productToBeDeleted.CurrentPrice,
                    Ean = productToBeDeleted.Ean,
                    ProductCategoryID = productToBeDeleted.ProductCategoryNavigationGuid,
                    ProductImage = productToBeDeleted.ProductImage
                };

                return View(model);
            }
            catch(ProductServiceDeleteException csDelete)
            {
                ModelState.AddModelError(string.Empty, csDelete.Message);
                return View();
            }
        }

        [HttpPost()]
        public IActionResult Delete(ProductDto p)
        {
            try
            {
                _deletableProductService.Delete(p.ProductName); 
            }
            catch (ProductServiceDeleteException csDelete)
            {
                ModelState.AddModelError(string.Empty, csDelete.Message);
                return View();
            }
            return RedirectToAction("Index");
        }

        // Use-case 4 
        [HttpGet()]
        public IActionResult Details(string id)
        {
            Product? product = _readOnlyProductService.ProductGetById(id);
            if(product == null)
            {
                throw new ProductNotFoundException();
            }

            return View(product);
        }

        // Use-case 5 get
        [HttpGet()]
        public IActionResult Edit(string id)
        {
            Product? product = _readOnlyProductService.ProductGetById(id);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            ViewBag.ProductCategories = _readOnlyProductCategoryService.GetAll();
            ProductDto model = new ProductDto()
            {
                ProductName = product.ProductName,
                CurrentPrice = product.CurrentPrice,
                Ean = product.Ean,
                ProductCategoryID = product.ProductCategoryNavigationGuid,
                ProductImage = product.ProductImage
            };

            return View(model);
        }

        // Use-case 5 poat
        [HttpPost()]
        public IActionResult Edit(ProductDto productDto)
        {
            try
            {
                if (productDto != null)
                {
                    Product? product = _readOnlyProductService.ProductGetById(productDto.ProductName);

                    if (product != null)
                    {
                        product.CurrentPrice = productDto.CurrentPrice;
                        product.Ean = productDto.Ean;
                        product.ProductCategoryNavigationGuid = productDto.ProductCategoryID;
                        product.ProductImage = productDto.ProductImage;

                        _updateableProductService.Update(product);
                    }

                }
            }
            catch(ProductServiceUpdateException csExc)
            {
                ModelState.AddModelError(string.Empty, csExc.Message);
                return View(productDto);
            }
            return RedirectToAction("Index");
        }
    }
}
