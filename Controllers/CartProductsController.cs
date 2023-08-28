using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.Repositories.UnitOfWork;

namespace PersonalShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET: api/CartProduct/{CartId}
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCartProducts(int cartId)
        {
            if (_unitOfWork.CartProducts == null)
            {
                return NotFound();
            }
            var result = (await _unitOfWork.CartProducts.GetProductsByCartId(cartId)).Select(cp => new CartProductDTO(cp)).ToList();

            return Ok(result);
        }

        //POST: api/CartProducts
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<CartProductDTO>> AddCartProduct(CartProductDTO cartProduct)
        {
            var cartProductToAdd = new CartProduct();
            cartProductToAdd.ProductId = cartProduct.ProductId;
            cartProductToAdd.CartId = cartProduct.CartId;
            cartProductToAdd.CartProductQuantity = cartProduct.CartProductQuantity;

            await _unitOfWork.CartProducts.Create(cartProductToAdd);
            _unitOfWork.Save();

            return Ok(cartProductToAdd);
        }

        //PUT: api/CartProducts
        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ModifyCartProductQuantity(int cartId, int productId, int newCartQuantity)
        {
            var existingCartProduct = await _unitOfWork.CartProducts.GetByComposedId(cartId, productId);

            if (existingCartProduct == null)
            { 
                return NotFound("Searched product not present in selected cart"); 
            }

            existingCartProduct.CartProductQuantity = newCartQuantity;

            await _unitOfWork.CartProducts.Update(existingCartProduct);
            _unitOfWork.Save();

            return Ok();
        }

        //DELETE: api/CartProducts
        [HttpDelete]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> DeleteCartProduct(int cartId, int productId)
        {
            var existingCartProduct = await _unitOfWork.CartProducts.GetByComposedId(cartId, productId);

            if (existingCartProduct == null)
            {
                return NotFound("Searched product not present in selected cart");
            }

            await _unitOfWork.CartProducts.Delete(existingCartProduct);
            _unitOfWork.Save();

            return Ok();
        }
    
    }
}
