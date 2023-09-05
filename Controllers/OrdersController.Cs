using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.Repositories.UnitOfWork;
using PersonalShopper.Services.OrderService;
using PersonalShopper.Services.UserService;
using System.Security.Claims;

namespace PersonalShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;

        public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
        }

        //GET: api/Orders/status
        [HttpGet("{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersWithSelectedStatus(string status)
        {
            var searchedOrders = await _unitOfWork.Orders.GetOrdersByStatus(status);

            if (searchedOrders == null || searchedOrders.Count() == 0)
            {
                return NotFound("No orders with this status");
            }

            return searchedOrders.Select(o => new OrderDTO(o)).ToList();
        }

        //GET: api/Orders/id/{orderId}
        [HttpGet("id/{orderId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int orderId)
        {
            var searchedOrder = await _unitOfWork.Orders.GetOrderById(orderId);

            if (searchedOrder == null)
            {
                return NotFound("No order registered with this id");
            }

            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var loggedUser = await _unitOfWork.Users.GetUserById(int.Parse(currentUserID));
            if (!searchedOrder.UserId.Equals(loggedUser.Id) && !loggedUser.UserRoles.Any(ur => ur.Role.Name == "Admin"))
            {
                return Forbid("Regular users can see only their orders");
            }

            return new OrderDTO(searchedOrder);
        }


        //GET: api/Orders/email/{userEmail}
        [HttpGet("email/{userEmail}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<OrderDTO>>> GetAllUserOrders(string userEmail)
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var loggedUser = await _unitOfWork.Users.GetUserById(int.Parse(currentUserID));
            if (!loggedUser.Email.Equals(userEmail) && !loggedUser.UserRoles.Any(ur => ur.Role.Name == "Admin"))
            {
                return Forbid("Regular users can see only their orders");
            }

            var userOrders = await _unitOfWork.Orders.GetAllUserOrders(loggedUser.Id);

            if (userOrders == null || userOrders.Count() == 0)
            {
                return NotFound("Your order history is empty");
            }

            return userOrders.Select(o => new OrderDTO(o)).ToList();

        }


        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderDTO>> PlaceUserOrder()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID == null)
            {
                return Forbid("No user currently logged in");
            }

            var loggedUser = await _unitOfWork.Users.GetUserById(int.Parse(currentUserID));
            if (loggedUser == null)
            {
                return NotFound("User not found");
            }

            var orderCart = await _unitOfWork.Carts.GetCartById(int.Parse(currentUserID));

            var orderToPlace = new Order();
            orderToPlace.UserId = loggedUser.Id;
            orderToPlace.User = loggedUser;
            orderToPlace.OrderPlaceDate = DateTime.Now;
            orderToPlace.OrderStatus = "Placed";

            var allOrderProducts = await _orderService.CreateOrderProducts(orderToPlace);
            
            if (allOrderProducts == null)
                return BadRequest("The cart is empty. You need at least 1 product in cart to place an order");

            orderToPlace.OrderProducts = allOrderProducts;

            await _unitOfWork.Orders.Create(orderToPlace);
            _unitOfWork.Save();
            var orderDTO = new OrderDTO(orderToPlace);
            return orderDTO;
        }

        //PUT: api/Orders
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDTO>> ModifyOrderStatus(int id, string newStatus)
        {
            var orderToModify = await _unitOfWork.Orders.GetOrderById(id);

            if (orderToModify == null)
            {
                return NotFound("No order registred with this id");
            }
       
            orderToModify.OrderStatus = newStatus;

            await _unitOfWork.Orders.Update(orderToModify);
            _unitOfWork.Save();

            return Ok();
        }

        //DELETE: api/Orders
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOrder (int id)
        {
            var searchedOrder = await _unitOfWork.Orders.GetOrderById(id);

            if (searchedOrder == null)
            {
                return NotFound("No order registred with this id");
            }

            await _unitOfWork.Orders.Delete(searchedOrder);
            _unitOfWork.Save();

            return Ok();
        }

    }
}
