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
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepository, 
            IAuthorRepository authorRepository,
            ICollectionRepository collectionRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _collectionRepository = collectionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Book>))]
        public IActionResult GetBooks()
        {
            var books = _mapper.Map<List<BookDto>>(_bookRepository.GetBooks());
             
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(books);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var book = _mapper.Map<BookDto>(_bookRepository.GetBook(bookId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateBook([FromQuery] int authorId, [FromBody] BookDto bookCreate)
        {
            if (bookCreate == null)
                return BadRequest(ModelState);

            var book = _bookRepository.GetBooks()
                .Where(b => b.Title.Trim().ToUpper() == bookCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (book != null)
            {
                ModelState.AddModelError("", "book already exists");
                return StatusCode(422, ModelState);
            }

            var author = _authorRepository.GetAuthor(authorId);

            if (author == null)
            {
                ModelState.AddModelError("", "author doesn't exist");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookMap = _mapper.Map<Book>(bookCreate);

            if (!_bookRepository.CreateBook(authorId, bookMap))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("author/{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddOwnership([FromQuery] int authorId, int bookId)
        {
            var book = _bookRepository.GetBook(bookId);

            if (book == null)
            {
                ModelState.AddModelError("", "book doesn't exist");
                return StatusCode(404, ModelState);
            }

            var author = _authorRepository.GetAuthor(authorId);

            if (author == null)
            {
                ModelState.AddModelError("", "author doesn't exist");
                return StatusCode(404, ModelState);
            }

            var bookInOwnership = _authorRepository.GetBooksByAuthor(authorId).Where(b => b.Id == bookId).FirstOrDefault();

            if (bookInOwnership != null)
            {
                ModelState.AddModelError("", "ownership already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookRepository.AddOwnership(authorId, bookId))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("collection/{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddIntoCollection([FromQuery] int collectionId, int bookId)
        {
            var book = _bookRepository.GetBook(bookId);

            if (book == null)
            {
                ModelState.AddModelError("", "book doesn't exist");
                return StatusCode(404, ModelState);
            }

            var collection = _collectionRepository.GetCollection(collectionId);

            if (collection == null)
            {
                ModelState.AddModelError("", "collection doesn't exist");
                return StatusCode(404, ModelState);
            }

            var bookInCollection = _collectionRepository.GetBooksByCollection(collectionId).Where(b => b.Id == bookId).FirstOrDefault();

            if (bookInCollection != null)
            {
                ModelState.AddModelError("", "book is already in this collection");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookRepository.AddIntoCollection(collectionId, bookId))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBook(int bookId, [FromBody] BookDto bookUpdate)
        {
            if (bookUpdate == null || bookId != bookUpdate.Id)
                return BadRequest(ModelState);

            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookMap = _mapper.Map<Book>(bookUpdate);

            if (!_bookRepository.UpdateBook(bookMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var book = _bookRepository.GetBook(bookId);

            if (book == null)
            {
                ModelState.AddModelError("", "book doesn't exist");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookRepository.DeleteBook(book))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
