using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Interfaces.RepositoryInterfaces;
using Xunit;
using LibraryApp.Services;
using AutoMapper;
using LibraryApp.Helper;
using LibraryApp.Dto;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Repository;

namespace LibraryApp.Tests.Integration.DataAccess
{
    public class NewsDataAccessTest
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;
        public readonly INewsRepository _newsRepository;

        public NewsDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _newsRepository = new NewsRepository(context);
            _mapper = mapper;
            _newsService = new NewsService(_newsRepository, _mapper);
        }

        [Fact]
        public void PostGetDeleteNewss()
        {
            var testNews = new NewsDto { Id = 9999, Title= "TEST", Description = "TEST", CreationDate = DateTime.UtcNow};

            _newsService.CreateNews(testNews);

            var news = _newsService.GetNewsById(testNews.Id);

            _newsService.DeleteNews(testNews.Id);

            Assert.Equivalent(testNews, news);
        }

        [Fact]
        public void GetNewss()
        {
            var testNews1 = new NewsDto { Id = 9990, Title = "TEST1", Description = "TEST", CreationDate = DateTime.UtcNow };
            var testNews2 = new NewsDto { Id = 9991, Title = "TEST2", Description = "TEST", CreationDate = DateTime.UtcNow };

            var newss = new List<NewsDto> { testNews1, testNews2 };

            _newsService.CreateNews(testNews1);
            _newsService.CreateNews(testNews2);

            var resultNewss = _newsService.GetNews();

            _newsService.DeleteNews(testNews1.Id);
            _newsService.DeleteNews(testNews2.Id);

            Assert.Equivalent(newss, resultNewss);
        }
    }
}
