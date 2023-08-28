using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.Repositories.UnitOfWork;

namespace PersonalShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET: api/Orders/status
        [HttpGet("{status}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersWithSelectedStatus(string status)
        {
            var searchedOrders = await _unitOfWork.Orders.GetOrdersByStatus(status);

            if (searchedOrders == null || searchedOrders.Count() == 0)
            {
                return NotFound("No orders with this status");
            }

            return searchedOrders.Select(o => new OrderDTO(o)).ToList();
        }

        //GET: api/Orders/id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var searchedOrder = await _unitOfWork.Orders.GetOrderById(id);

            if (searchedOrder == null)
            {
                return NotFound("No order registered with this id");
            }

            return new OrderDTO(searchedOrder);
        }


        //GET: api/Orders/userId
        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<OrderDTO>>> GetAllUserOrders(int id)
        {
            var userOrders = await _unitOfWork.Orders.GetAllUserOrders(id);

            if (userOrders == null || userOrders.Count() == 0)
            {
                return NotFound("Your order history is empty");
            }

            return userOrders.Select(o => new OrderDTO(o)).ToList();

        }


        //POST: api/Orders
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<OrderDTO>> PlaceUserOrder(OrderDTO order)
        {
            var orderToPlace = new Order();
            orderToPlace.OrderId = order.OrderId;
            orderToPlace.UserId = order.UserId;
            orderToPlace.CartId = order.CartId;
            orderToPlace.OrderStatus = order.OrderStatus;

            await _unitOfWork.Orders.Create(orderToPlace);
            _unitOfWork.Save();

            return Ok();
        }

        //PUT: api/Orders
        [HttpPut]
        [Authorize(Roles = "Admin,User")]
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
        [Authorize(Roles = "Admin,User")]
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
