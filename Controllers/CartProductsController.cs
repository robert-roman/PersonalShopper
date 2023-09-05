using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.Repositories.UnitOfWork;
using System.Security.Claims;

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

        //GET: api/CartProduct/
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetProductsFromCart()
        {
            var currentUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (_unitOfWork.CartProducts == null)
            {
                return NotFound();
            }
            var result = (await _unitOfWork.CartProducts.GetProductsByCartId(currentUserID)).Select(cp => new CartProductDTO(cp)).ToList();

            return Ok(result);
        }

        //GET: api/CartProduct/{cartId}
        [HttpGet("{cartId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductsFromCart(int cartId)
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
        public async Task<ActionResult<CartProductDTO>> AddCartProduct(int productId, int productQuantity)
        {

            var searchedProduct = await _unitOfWork.Products.GetById(productId);
            if (searchedProduct == null)
            {
                return NotFound("No product registred with this id");
            }    


            var currentUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var userCart = await _unitOfWork.Carts.GetCartById(currentUserID);

            var existingCartProduct = await _unitOfWork.CartProducts.GetByComposedId(currentUserID, productId);
            if (existingCartProduct != null)
            {
                existingCartProduct.CartProductQuantity += productQuantity;
                await _unitOfWork.CartProducts.Update(existingCartProduct);
                _unitOfWork.Save();

                var cpDTO = new CartProductDTO(existingCartProduct);
                return cpDTO;
            }

            else
            {
                var cartProductToAdd = new CartProduct();
                cartProductToAdd.ProductId = productId;
                cartProductToAdd.CartId = currentUserID;
                cartProductToAdd.CartProductQuantity = productQuantity;
                cartProductToAdd.Cart = userCart;
                cartProductToAdd.Product = searchedProduct;

                await _unitOfWork.CartProducts.Create(cartProductToAdd);

                userCart.CartProducts.Add(cartProductToAdd);
                await _unitOfWork.Carts.Update(userCart);
                _unitOfWork.Save();

                var cpDTO = new CartProductDTO(cartProductToAdd);
                return cpDTO;

            }
        }

        //PUT: api/CartProducts
        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ModifyCartProductQuantity(int productId, int newCartQuantity)
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var existingCartProduct = await _unitOfWork.CartProducts.GetByComposedId(int.Parse(currentUserID), productId);

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
        [Authorize(Roles ="User")]
        public async Task<IActionResult> RemoveProductFromCart(int productId)
        {
            var currentUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var existingCartProduct = await _unitOfWork.CartProducts.GetByComposedId(currentUserID, productId);

            if (existingCartProduct == null)
            {
                return NotFound("Searched product not present in selected cart");
            }

            var userCart = await _unitOfWork.Carts.GetCartById(currentUserID);
            userCart.CartProducts.Remove(existingCartProduct);
            await _unitOfWork.Carts.Update(userCart);
            await _unitOfWork.CartProducts.Delete(existingCartProduct);
            _unitOfWork.Save();

            return Ok();
        }

        //DELETE: api/CartProducts/cartId/{cartId}
        [HttpDelete("cartId/{cartId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProductFromUserCart(int productId, int cartId)
        {


            var existingCartProduct = await _unitOfWork.CartProducts.GetByComposedId(cartId, productId);

            if (existingCartProduct == null)
            {
                return NotFound("Searched product not present in selected cart");
            }

            var userCart = await _unitOfWork.Carts.GetCartById(cartId);
            userCart.CartProducts.Remove(existingCartProduct);
            await _unitOfWork.Carts.Update(userCart);
            await _unitOfWork.CartProducts.Delete(existingCartProduct);
            _unitOfWork.Save();

            return Ok();
        }

    }
}
