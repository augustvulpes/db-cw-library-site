using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public IActionResult GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDto>>(_authorRepository.GetAuthors());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var author = _mapper.Map<AuthorDto>(_authorRepository.GetAuthor(authorId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(author);
        }

        [HttpGet("book/{authorId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var books = _mapper.Map<List<BookDto>>(_authorRepository.GetBooksByAuthor(authorId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAuthor([FromBody] AuthorDto authorCreate)
        {
            if (authorCreate == null)
                return BadRequest(ModelState);

            var author = _authorRepository.GetAuthors()
                .Where(a => a.Name.Trim().ToUpper() == authorCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (author != null)
            {
                ModelState.AddModelError("", "author already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorMap = _mapper.Map<Author>(authorCreate);

            if (!_authorRepository.CreateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAuthor(int authorId, [FromBody] AuthorDto authorUpdate)
        {
            if (authorUpdate == null || authorId != authorUpdate.Id)
                return BadRequest(ModelState);

            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorMap = _mapper.Map<Author>(authorUpdate);

            if (!_authorRepository.UpdateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var author = _authorRepository.GetAuthor(authorId);

            if (author == null)
            {
                ModelState.AddModelError("", "author doesn't exist");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.DeleteAuthor(author))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

