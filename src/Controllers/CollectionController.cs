using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces;
using LibraryApp.Models;
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
    }
}
