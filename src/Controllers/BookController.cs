using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Book>))]
        public IActionResult GetBooks()
        {
            var books = _bookService.GetBooks();
             
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(books);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]
        public IActionResult GetBook(int bookId)
        {
            try
            {
                var book = _bookService.GetBook(bookId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(book);
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
        public IActionResult CreateBook([FromQuery] int authorId, [FromBody] BookDto bookCreate)
        {
            try
            {
                var response = _bookService.CreateBook(authorId, bookCreate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(422, new { message = e.Message });
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

        [HttpPost("author/{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddOwnership([FromQuery] int authorId, int bookId)
        {
            try
            {
                var response = _bookService.AddOwnership(authorId, bookId);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(422, new { message = e.Message });
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

        [HttpPost("collection/{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddIntoCollection([FromQuery] int collectionId, int bookId)
        {
            try
            {
                var response = _bookService.AddIntoCollection(collectionId, bookId);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(422, new { message = e.Message });
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

        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBook(int bookId, [FromBody] BookDto bookUpdate)
        {
            try
            {
                var response = _bookService.UpdateBook(bookId, bookUpdate);

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

        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                var response = _bookService.DeleteBook(bookId);

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
