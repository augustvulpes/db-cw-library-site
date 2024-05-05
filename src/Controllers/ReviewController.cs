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
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review =_mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var review = _userRepository.GetReviewsByUser(reviewCreate.UserId)
                .Where(r => r.BookId == reviewCreate.BookId)
                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "this user already has a review on this book");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.User = _userRepository.GetUser(reviewCreate.UserId);
            reviewMap.Book = _bookRepository.GetBook(reviewCreate.BookId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewUpdate)
        {
            if (reviewUpdate == null || reviewId != reviewUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewUpdate);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
