using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.Repositories.UnitOfWork;

namespace PersonalShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET: api/Products
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            if (_unitOfWork.Products == null)
            {
                return NotFound();
            }
            var results = (await _unitOfWork.Products.GetAll()).Select(p => new ProductDTO(p)).ToList();
            return results;
        }

        //GET: api/Products/productName/{productName}
        [HttpGet("productName/{productName}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDTO>> GetProductByName(string productName)
        {
            var searchedProduct = await _unitOfWork.Products.GetProductByName(productName);

            if (searchedProduct == null)
            {
                return NotFound("Product not listed");
            }

            return new ProductDTO(searchedProduct);
        }

        //GET: api/Products/category/{productCategory}
        [HttpGet("category/{productCategory}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsWithCategory(string productCategory)
        {
            var searchedProducts = await _unitOfWork.Products.GetProductsWithCategory(productCategory);

            if (searchedProducts == null || searchedProducts.Count == 0)
            {
                return NotFound("Category not listed or no searched product available");
            }

            return searchedProducts.Select(p => new ProductDTO(p)).ToList();
        }

        //GET: api/Products/description/{productDescription}
        [HttpGet("description/{productDescription}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsContainingDescription(string productDescription)
        {
            var searchedProducts = await _unitOfWork.Products.GetProductsContainingDescription(productDescription);

            if (searchedProducts == null || searchedProducts.Count == 0)
            {
                return NotFound("Description not matching with any existing");

            }

            return searchedProducts.Select(p => new ProductDTO(p)).ToList();
        }


        //GET: api/Products/SeePricesAndSales
        [HttpGet("SeePricesAndSales")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<(int ProductId, string ProductName, float ProductPrice,
                                            float OrderProductPrice, int OrderProductQuantity)>>>
                         SeePricesAndSales()
        {
            return await _unitOfWork.OrderProducts.ComparePreviousPricesForProducts();
        }



        //POST: api/Products
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ProductDTO>> PostProducts(ProductDTO product)
        {
            var ProductToAdd = new Product();
            ProductToAdd.ProductName = product.ProductName;
            ProductToAdd.ProductCategory = product.ProductCategory;
            ProductToAdd.ProductBrand = product.ProductBrand;
            ProductToAdd.ProductDescription = product.ProductDescription;
            ProductToAdd.ProductPrice = product.ProductPrice;
            ProductToAdd.ProductStock = product.ProductStock;

            await _unitOfWork.Products.Create(ProductToAdd);
            _unitOfWork.Save();

            return Ok();
        }

        //PUT: api/Products/id/{productId}
        [HttpPut("id/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProduct(int productId, ProductDTO product)
        {
            var searchedProduct = await _unitOfWork.Products.GetById(productId);

            if (searchedProduct == null)
            {
                return NotFound("Product with specified id doesn't exist");
            }

            searchedProduct.ProductDescription = product.ProductDescription;
            searchedProduct.ProductPrice = product.ProductPrice;
            searchedProduct.ProductStock = product.ProductStock;

            await _unitOfWork.Products.Update(searchedProduct);
            _unitOfWork.Save();

            return Ok();
        }

        //DELETE: api/Products/id/{productId}
        [HttpDelete("id/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var searchedProduct = await _unitOfWork.Products.GetById(productId);

            if (searchedProduct == null)
            {
                return NotFound("Product with specified id doesn't exist");
            }

            await _unitOfWork.Products.Delete(searchedProduct);
            _unitOfWork.Save();

            return Ok();
        }
    }
}
