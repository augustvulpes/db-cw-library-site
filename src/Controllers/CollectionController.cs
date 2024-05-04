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
    public class CollectionController : Controller
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IMapper _mapper;

        public CollectionController(ICollectionRepository collectionRepository, IMapper mapper)
        {
            _collectionRepository = collectionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Collection>))]
        public IActionResult GetCollections()
        {
            var collections = _mapper.Map<List<CollectionDto>>(_collectionRepository.GetCollections());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(collections);
        }

        [HttpGet("{collectionId}")]
        [ProducesResponseType(200, Type = typeof(Collection))]
        [ProducesResponseType(400)]
        public IActionResult GetCollection(int collectionId)
        {
            if (!_collectionRepository.CollectionExists(collectionId))
                return NotFound();

            var collection = _mapper.Map<CollectionDto>(_collectionRepository.GetCollection(collectionId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(collection);
        }

        [HttpGet("book/{collectionId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooksByCollectionId(int collectionId)
        {
            var books = _mapper.Map<List<BookDto>>(_collectionRepository.GetBooksByCollection(collectionId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCollection([FromBody] CollectionDto collectionCreate)
        {
            if (collectionCreate == null)
                return BadRequest(ModelState);

            var collection = _collectionRepository.GetCollections()
                .Where(a => a.Title.Trim().ToUpper() == collectionCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (collection != null)
            {
                ModelState.AddModelError("", "collection already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collectionMap = _mapper.Map<Collection>(collectionCreate);

            if (!_collectionRepository.CreateCollection(collectionMap))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
