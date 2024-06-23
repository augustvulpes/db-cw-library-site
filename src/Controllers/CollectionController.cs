using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;
        private readonly ILogger<CollectionController> _logger;

        public CollectionController(ICollectionService collectionService, ILogger<CollectionController> logger)
        {
            _collectionService = collectionService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Collection>))]
        public IActionResult GetCollections()
        {
            var collections = _collectionService.GetCollections();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(collections);
        }

        [HttpGet("{collectionId}")]
        [ProducesResponseType(200, Type = typeof(Collection))]
        [ProducesResponseType(400)]
        public IActionResult GetCollection(int collectionId)
        {
            try
            {
                var collection = _collectionService.GetCollection(collectionId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(collection);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet("book/{collectionId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooksByCollectionId(int collectionId)
        {
            var books = _collectionService.GetBooksByCollectionId(collectionId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCollection([FromBody] CollectionDto collectionCreate)
        {
            try
            {
                var response = _collectionService.CreateCollection(collectionCreate);

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
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }
        }

        [HttpPut("{collectionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCollection(int collectionId, [FromBody] CollectionDto collectionUpdate)
        {
            try
            {
                var response = _collectionService.UpdateCollection(collectionId, collectionUpdate);

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

        [HttpDelete("{collectionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCollection(int collectionId)
        {
            try
            {
                var response = _collectionService.DeleteCollection(collectionId);

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
