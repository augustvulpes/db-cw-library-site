﻿using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<News>))]
        public IActionResult GetNews()
        {
            var news = _newsService.GetNews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(news);
        }

        [HttpGet("{newsId}")]
        [ProducesResponseType(200, Type = typeof(News))]
        [ProducesResponseType(400)]
        public IActionResult GetNewsById(int newsId)
        {
            try
            {
                var news = _newsService.GetNewsById(newsId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(news);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateNews([FromBody] NewsDto newsCreate)
        {
            try
            {
                var response = _newsService.CreateNews(newsCreate);

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

        [HttpPut("{newsId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateNews(int newsId, [FromBody] NewsDto newsUpdate)
        {
            try
            {
                var response = _newsService.UpdateNews(newsId, newsUpdate);

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

        [HttpDelete("{newsId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteNews(int newsId)
        {
            try
            {
                var response = _newsService.DeleteNews(newsId);

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
