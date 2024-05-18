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
    public class ReviewDataAccessTest
    {
        private readonly IReviewService _reviewService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public readonly IReviewRepository _reviewRepository;
        public readonly IBookRepository _bookRepository;
        public readonly IUserRepository _userRepository;
        public readonly IAuthorRepository _authorRepository;
        public readonly ICollectionRepository _collectionRepository;

        public ReviewDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _reviewRepository = new ReviewRepository(context);
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
        }

        [Fact]
        public void PostGetDeleteReviews()
        {
            var testReview = new ReviewDto { Id = 9999, BookId = 1, UserId = "1", CreationDate = DateTime.UtcNow, Content="TEST" };

            _reviewService.CreateReview(testReview);

            var review = _reviewService.GetReview(testReview.Id);

            _reviewService.DeleteReview(testReview.Id);

            Assert.Equivalent(testReview, review);
        }

        [Fact]
        public void GetReviews()
        {
            var testReview1 = new ReviewDto { Id = 9990, BookId = 1, UserId = "1", CreationDate = DateTime.UtcNow, Content = "TEST" };
            var testReview2 = new ReviewDto { Id = 9991, BookId = 1, UserId = "2", CreationDate = DateTime.UtcNow, Content = "TEST" };

            var reviews = new List<ReviewDto> { testReview1, testReview2 };

            _reviewService.CreateReview(testReview1);
            _reviewService.CreateReview(testReview2);

            var resultReviews = _reviewService.GetReviews();

            _reviewService.DeleteReview(testReview1.Id);
            _reviewService.DeleteReview(testReview2.Id);

            Assert.Equivalent(reviews, resultReviews);
        }
    }
}
