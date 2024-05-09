using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsService(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public List<NewsDto> GetNews()
        {
            var newss = _mapper.Map<List<NewsDto>>(_newsRepository.GetNews());

            return newss;
        }

        public NewsDto GetNewsById(int newsId)
        {
            if (!_newsRepository.NewsExists(newsId))
                throw new NotFoundException("News not found");

            var News = _mapper.Map<NewsDto>(_newsRepository.GetNewsById(newsId));

            return News;
        }

        public string CreateNews(NewsDto newsCreate)
        {
            if (newsCreate == null)
                throw new BadRequestException();

            var news = _newsRepository.GetNews()
                .Where(a => a.Title.Trim().ToUpper() == newsCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (news != null)
                throw new UnprocessableException("News already exists");

            var newsMap = _mapper.Map<News>(newsCreate);

            if (!_newsRepository.CreateNews(newsMap))
                throw new Exception();

            return "Successfully created";
        }

        public string UpdateNews(int newsId, NewsDto newsUpdate)
        {
            if (newsUpdate == null || newsId != newsUpdate.Id)
                throw new BadRequestException();

            if (!_newsRepository.NewsExists(newsId))
                throw new NotFoundException("News not found");

            var newsMap = _mapper.Map<News>(newsUpdate);

            if (!_newsRepository.UpdateNews(newsMap))
                throw new Exception();

            return "Successfully updated";
        }

        public string DeleteNews(int newsId)
        {
            var news = _newsRepository.GetNewsById(newsId);

            if (news == null)
                throw new NotFoundException("News not found");

            if (!_newsRepository.DeleteNews(news))
                throw new Exception();

            return "Successfully deleted";
        }
    }
}
