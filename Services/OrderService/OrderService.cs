using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using System.Security.Claims;
using PersonalShopper.Repositories.UnitOfWork;

namespace PersonalShopper.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ICollection<OrderProduct>?> CreateOrderProducts(Order order)
        {
            if (order.User.Cart.CartProducts.Count() == 0)
            {
                Console.WriteLine("You need to have at least one product in cart to place an order");
                return null;
            }

            ICollection<OrderProduct> allOrderProduct = new List<OrderProduct>();
            foreach (CartProduct cartProduct in order.User.Cart.CartProducts)
            {
                var cartProductDTO = new CartProductDTO(cartProduct);
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.OrderId,
                    ProductId = cartProductDTO.ProductId,
                    ProductName = cartProductDTO.ProductName,
                    ProductPrice = cartProductDTO.ProductPrice,
                    OrderProductQuantity = cartProductDTO.CartProductQuantity
                };
                //await _unitOfWork.OrderProducts.Create(orderProduct);
                allOrderProduct.Add(orderProduct);
            }
            //_unitOfWork.Save();
            return allOrderProduct;
        }
    }
}
