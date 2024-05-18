using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Interfaces.RepositoryInterfaces;
using Xunit;
using LibraryApp.Services;
using AutoMapper;
using LibraryApp.Helper;
using LibraryApp.Dto;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Repository;

namespace LibraryApp.Tests.Integration.DataAccess
{
    public class OrderDataAccessTest
    {
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public readonly IOrderRepository _orderRepository;
        public readonly IBookRepository _bookRepository;
        public readonly IUserRepository _userRepository;
        public readonly IAuthorRepository _authorRepository;
        public readonly ICollectionRepository _collectionRepository;

        public OrderDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _orderRepository = new OrderRepository(context);
            _bookRepository = new BookRepository(context);
            _userRepository = new UserRepository(context);
            _authorRepository = new AuthorRepository(context);
            _collectionRepository = new CollectionRepository(context);
            _mapper = mapper;

            _bookService = new BookService(_bookRepository,
                _authorRepository,
                _collectionRepository,
                _mapper);
            _userService = new UserService(_userRepository, mapper, null, null);
            _orderService = new OrderService(_orderRepository,
                _userRepository,
                _bookRepository,
                _mapper);
        }

        [Fact]
        public void PostGetDeleteOrders()
        {
            var testOrder = new OrderDto { Id = 9999, BookId = 1, UserId = "1", CreationDate = DateTime.UtcNow, State = "new" };

            _orderService.CreateOrder(testOrder);

            var order = _orderService.GetOrder(testOrder.Id);

            _orderService.DeleteOrder(testOrder.Id);

            Assert.Equivalent(testOrder, order);
        }

        [Fact]
        public void GetOrders()
        {
            var testOrder1 = new OrderDto { Id = 9990, BookId = 1, UserId = "1", CreationDate = DateTime.UtcNow, State = "new" };
            var testOrder2 = new OrderDto { Id = 9991, BookId = 1, UserId = "2", CreationDate = DateTime.UtcNow, State = "new" };

            var orders = new List<OrderDto> { testOrder1, testOrder2 };

            _orderService.CreateOrder(testOrder1);
            _orderService.CreateOrder(testOrder2);

            var resultOrders = _orderService.GetAllOrders();

            _orderService.DeleteOrder(testOrder1.Id);
            _orderService.DeleteOrder(testOrder2.Id);

            Assert.Equivalent(orders, resultOrders);
        }
    }
}
