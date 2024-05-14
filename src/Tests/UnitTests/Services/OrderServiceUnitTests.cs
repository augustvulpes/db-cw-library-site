using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Interfaces.RepositoryInterfaces;
using Xunit;
using Moq;
using LibraryApp.Models;
using LibraryApp.Services;
using AutoMapper;
using LibraryApp.Helper;
using LibraryApp.Dto;

namespace LibraryApp.Tests.UnitTests.Services
{
    public class OrderServiceUnitTests
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        public readonly Mock<IBookRepository> _bookRepositoryMock = new();
        public readonly Mock<IUserRepository> _userRepositoryMock = new();

        public OrderServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = config.CreateMapper();

            _orderService = new OrderService(_orderRepositoryMock.Object,
                _userRepositoryMock.Object,
                _bookRepositoryMock.Object,
                mapper);
            _mapper = mapper;
        }

        [Fact]
        public void GetAllOrders()
        {
            var testOrders = new List<Order>
            {
                new Order { Id = 1, BookId = 1, UserId = "1", State = "New", CreationDate = DateTime.Today },
                new Order { Id = 2, BookId = 2, UserId = "2", State = "New", CreationDate = DateTime.Today }
            };

            _orderRepositoryMock.Setup(r => r.GetAllOrders()).Returns(testOrders);

            var resultOrders = _mapper.Map<List<Order>>(_orderService.GetAllOrders());

            Assert.Equivalent(testOrders, resultOrders);
        }

        [Fact]
        public void GetOrder()
        {
            var testOrder = new Order { Id = 1, BookId = 1, UserId = "1", State = "New", CreationDate = DateTime.Today };

            _orderRepositoryMock.Setup(r => r.OrderExists(It.IsAny<int>())).Returns(true);
            _orderRepositoryMock.Setup(r => r.GetOrder(It.IsAny<int>())).Returns(testOrder);

            var resultOrder = _mapper.Map<Order>(_orderService.GetOrder(1));

            Assert.Equivalent(testOrder, resultOrder);
        }

        [Fact]
        public void CreateOrder()
        {
            var orderCreate = new OrderDto { Id = 1, BookId = 1, UserId = "1", State = "New", CreationDate = DateTime.Today };
            var testBook = new Book { Id = 1, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };
            var testUser = new User { Id = "1" };

            _bookRepositoryMock.Setup(r => r.GetBook(It.IsAny<int>())).Returns(testBook);
            _userRepositoryMock.Setup(r => r.GetOrdersByUser(It.IsAny<string>())).Returns(new List<Order> { });
            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<string>())).Returns(testUser);
            _orderRepositoryMock.Setup(r => r.CreateOrder(It.IsAny<Order>())).Returns(true);

            var result = _orderService.CreateOrder(orderCreate);

            Assert.Equivalent("Successfully created", result);
        }

        [Fact]
        public void UpdateOrder()
        {
            var orderUpdate = new OrderDto { Id = 1, BookId = 1, UserId = "1", State = "New", CreationDate = DateTime.Today };

            _orderRepositoryMock.Setup(r => r.OrderExists(It.IsAny<int>())).Returns(true);
            _orderRepositoryMock.Setup(r => r.UpdateOrder(It.IsAny<Order>())).Returns(true);

            var result = _orderService.UpdateOrder(1, orderUpdate);

            Assert.Equivalent("Successfully updated", result);
        }

        [Fact]
        public void DeleteOrder()
        {
            var testOrder = new Order { Id = 1, BookId = 1, UserId = "1", State = "New", CreationDate = DateTime.Today };

            _orderRepositoryMock.Setup(r => r.GetOrder(It.IsAny<int>())).Returns(testOrder);
            _orderRepositoryMock.Setup(r => r.DeleteOrder(testOrder)).Returns(true);

            var result = _orderService.DeleteOrder(1);

            Assert.Equivalent("Successfully deleted", result);
        }
    }
}
