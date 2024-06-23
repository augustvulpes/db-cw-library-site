using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var review = _reviewService.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            try
            {
                var review = _reviewService.GetReview(reviewId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(review);
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
        public IActionResult CreateReview([FromBody] ReviewDto reviewCreate)
        {
            try
            {
                var response = _reviewService.CreateReview(reviewCreate);

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
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewUpdate)
        {
            try
            {
                var response = _reviewService.UpdateReview(reviewId, reviewUpdate);

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

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            try
            {
                var response = _reviewService.DeleteReview(reviewId);

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
