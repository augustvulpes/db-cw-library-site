using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public List<OrderDto> GetAllOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetAllOrders());

            return orders;
        }

        public List<OrderDto> GetNewOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetNewOrders());

            return orders;
        }

        public OrderDto GetOrder(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                throw new NotFoundException("Order not found");

            var Order = _mapper.Map<OrderDto>(_orderRepository.GetOrder(orderId));

            return Order;
        }

        public string CreateOrder(OrderDto orderCreate)
        {
            if (orderCreate == null)
                throw new BadRequestException();

            var order = _userRepository.GetOrdersByUser(orderCreate.UserId)
                .Where(o => o.BookId == orderCreate.BookId)
                .FirstOrDefault();

            if (order != null)
                throw new UnprocessableException("This user already has an order on this book");

            var orderMap = _mapper.Map<Order>(orderCreate);

            orderMap.User = _userRepository.GetUser(orderCreate.UserId);
            orderMap.Book = _bookRepository.GetBook(orderCreate.BookId);

            if (orderMap.User == null || orderMap.Book == null)
                throw new NotFoundException("Not found");

            if (!_orderRepository.CreateOrder(orderMap))
                throw new Exception();

            return "Successfully created";
        }

        public string UpdateOrder(int orderId, OrderDto orderUpdate)
        {
            if (orderUpdate == null || orderId != orderUpdate.Id)
                throw new BadRequestException();

            if (!_orderRepository.OrderExists(orderId))
                throw new NotFoundException("Order not found");

            var orderMap = _mapper.Map<Order>(orderUpdate);

            if (!_orderRepository.UpdateOrder(orderMap))
                throw new Exception();

            return "Successfully updated";
        }

        public string DeleteOrder(int orderId)
        {
            var order = _orderRepository.GetOrder(orderId);

            if (order == null)
                throw new NotFoundException("Order not found");

            if (!_orderRepository.DeleteOrder(order))
                throw new Exception();

            return "Successfully deleted";
        }
    }
}
