using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public IActionResult GetAuthors()
        {
            var authors = _authorService.GetAuthors();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthor(int authorId)
        {
            try
            {
                var author = _authorService.GetAuthor(authorId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(author);
            }
            catch (NotFoundException e)
            {
                return NotFound(new {message = e.Message});
            }
        }

        [HttpGet("book/{authorId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var books = _authorService.GetBooksByAuthorId(authorId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAuthor([FromBody] AuthorDto authorCreate)
        {
            try
            {
                var response = _authorService.CreateAuthor(authorCreate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception _)
            {
                ModelState.AddModelError("", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAuthor(int authorId, [FromBody] AuthorDto authorUpdate)
        {
            try
            {
                var response = _authorService.UpdateAuthor(authorId, authorUpdate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                return BadRequest();
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception _)
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAuthor(int authorId)
        {
            try
            {
                var response = _authorService.DeleteAuthor(authorId);

                return Ok(response);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception _)
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
        }
    }
}

