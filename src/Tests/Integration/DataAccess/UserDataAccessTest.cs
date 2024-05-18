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
    public class UserDataAccessTest
    {
        private readonly IReviewService _reviewService;
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public readonly IReviewRepository _reviewRepository;
        public readonly IBookRepository _bookRepository;
        public readonly IUserRepository _userRepository;
        public readonly IAuthorRepository _authorRepository;
        public readonly ICollectionRepository _collectionRepository;
        public readonly IOrderRepository _orderRepository;

        public UserDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _reviewRepository = new ReviewRepository(context);
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
            _reviewService = new ReviewService(_reviewRepository,
                _userRepository,
                _bookRepository,
                _mapper);
            _orderService = new OrderService(_orderRepository,
                _userRepository,
                _bookRepository,
                _mapper);
        }

        [Fact]
        public void GetOrdersPerUser()
        {
            var testOrder1 = new OrderDto { Id = 9990, BookId = 1, UserId = "1", CreationDate = DateTime.UtcNow, State = "new" };
            var testOrder2 = new OrderDto { Id = 9991, BookId = 2, UserId = "1", CreationDate = DateTime.UtcNow, State = "new" };

            var orders = new List<OrderDto> { testOrder1, testOrder2 };

            _orderService.CreateOrder(testOrder1);
            _orderService.CreateOrder(testOrder2);

            var resultOrders = _userService.GetOrdersByUser(testOrder1.UserId);

            _orderService.DeleteOrder(testOrder1.Id);
            _orderService.DeleteOrder(testOrder2.Id);

            Assert.Equivalent(orders, resultOrders);
        }

        [Fact]
        public void GetReviewsPerUser()
        {
            var testReview1 = new ReviewDto { Id = 9990, BookId = 1, UserId = "1", CreationDate = DateTime.UtcNow, Content = "TEST" };
            var testReview2 = new ReviewDto { Id = 9991, BookId = 2, UserId = "1", CreationDate = DateTime.UtcNow, Content = "TEST" };

            var reviews = new List<ReviewDto> { testReview1, testReview2 };

            _reviewService.CreateReview(testReview1);
            _reviewService.CreateReview(testReview2);

            var resultReviews = _userRepository.GetReviewsByUser(testReview1.UserId);

            _reviewService.DeleteReview(testReview1.Id);
            _reviewService.DeleteReview(testReview2.Id);

            Assert.Equivalent(reviews, resultReviews);
        }
    }
}
