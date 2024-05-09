using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public List<ReviewDto> GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            return reviews;
        }

        public ReviewDto GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                throw new NotFoundException("Review not found");

            var Review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            return Review;
        }

        public string CreateReview(ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                throw new BadRequestException();

            var review = _userRepository.GetReviewsByUser(reviewCreate.UserId)
                .Where(o => o.BookId == reviewCreate.BookId)
                .FirstOrDefault();

            if (review != null)
                throw new UnprocessableException("This user already has a review on this book");

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.User = _userRepository.GetUser(reviewCreate.UserId);
            reviewMap.Book = _bookRepository.GetBook(reviewCreate.BookId);

            if (reviewMap.User == null || reviewMap.Book == null)
                throw new NotFoundException("Not found");

            if (!_reviewRepository.CreateReview(reviewMap))
                throw new Exception();

            return "Successfully created";
        }

        public string UpdateReview(int reviewId, ReviewDto reviewUpdate)
        {
            if (reviewUpdate == null || reviewId != reviewUpdate.Id)
                throw new BadRequestException();

            if (!_reviewRepository.ReviewExists(reviewId))
                throw new NotFoundException("Review not found");

            var reviewMap = _mapper.Map<Review>(reviewUpdate);

            if (!_reviewRepository.UpdateReview(reviewMap))
                throw new Exception();

            return "Successfully updated";
        }

        public string DeleteReview(int reviewId)
        {
            var review = _reviewRepository.GetReview(reviewId);

            if (review == null)
                throw new NotFoundException("Review not found");

            if (!_reviewRepository.DeleteReview(review))
                throw new Exception();

            return "Successfully deleted";
        }
    }
}
