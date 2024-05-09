using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface INewsService
    {
        public List<NewsDto> GetNews();
        public NewsDto GetNewsById(int newsId);
        public string CreateNews(NewsDto newsCreate);
        public string UpdateNews(int newsId, NewsDto newsUpdate);
        public string DeleteNews(int newsId);
    }
}
