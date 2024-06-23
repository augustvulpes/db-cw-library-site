using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpGet("new/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetNewOrders()
        {
            var orders = _orderService.GetNewOrders();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetOrder(int orderId)
        {
            try
            {
                var order = _orderService.GetOrder(orderId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(order);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromBody] OrderDto orderCreate)
        {
            try
            {
                var response = _orderService.CreateOrder(orderCreate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (UnprocessableException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }
        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOrder(int orderId, [FromBody] OrderDto orderUpdate)
        {
            try
            {
                var response = _orderService.UpdateOrder(orderId, orderUpdate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder(int orderId)
        {
            try
            {
                var response = _orderService.DeleteOrder(orderId);

                return Ok(response);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
        }
    }
}
