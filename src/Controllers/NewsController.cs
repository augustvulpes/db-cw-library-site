using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsController(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<News>))]
        public IActionResult GetNews()
        {
            var news = _mapper.Map<List<NewsDto>>(_newsRepository.GetNews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(news);
        }

        [HttpGet("{newsId}")]
        [ProducesResponseType(200, Type = typeof(News))]
        [ProducesResponseType(400)]
        public IActionResult GetNewsById(int newsId)
        {
            if (!_newsRepository.NewsExists(newsId))
                return NotFound();

            var news = _mapper.Map<NewsDto>(_newsRepository.GetNewsById(newsId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(news);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateNews([FromBody] NewsDto newsCreate)
        {
            if (newsCreate == null)
                return BadRequest(ModelState);

            var news = _newsRepository.GetNews()
                .Where(n => n.Title.Trim().ToUpper() == newsCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (news != null)
            {
                ModelState.AddModelError("", "news already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newsMap = _mapper.Map<News>(newsCreate);

            if (!_newsRepository.CreateNews(newsMap))
            {
                ModelState.AddModelError("", "Something went wrog while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
