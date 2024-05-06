using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Repository;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetAllOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetAllOrders());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpGet("new/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetNewOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetNewOrders());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetOrder(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound();

            var order = _mapper.Map<OrderDto>(_orderRepository.GetOrder(orderId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromBody] OrderDto orderCreate)
        {
            if (orderCreate == null)
                return BadRequest(ModelState);

            var order = _userRepository.GetOrdersByUser(orderCreate.UserId)
                .Where(o => o.BookId == orderCreate.BookId)
                .FirstOrDefault();

            if (order != null)
            {
                ModelState.AddModelError("", "this user already has an order on this book");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderMap = _mapper.Map<Order>(orderCreate);

            orderMap.User = _userRepository.GetUser(orderCreate.UserId);
            orderMap.Book = _bookRepository.GetBook(orderCreate.BookId);

            if (orderMap.User == null || orderMap.Book == null)
                return StatusCode(404, ModelState);

            if (!_orderRepository.CreateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOrder(int orderId, [FromBody] OrderDto orderUpdate)
        {
            if (orderUpdate == null || orderId != orderUpdate.Id)
                return BadRequest(ModelState);

            if (!_orderRepository.OrderExists(orderId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderMap = _mapper.Map<Order>(orderUpdate);

            if (!_orderRepository.UpdateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound();

            var order = _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                ModelState.AddModelError("", "order doesn't exist");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_orderRepository.DeleteOrder(order))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
