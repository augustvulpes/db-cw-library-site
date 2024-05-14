using LibraryApp.Data;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Models;

namespace LibraryApp.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly DataContext _context;
        public NewsRepository(DataContext context)
        {
            _context = context;
        }

        public List<News> GetNews()
        {
            return _context.News.ToList();
        }

        public News GetNewsById(int id)
        {
            return _context.News.Where(n =>  n.Id == id).FirstOrDefault();
        }

        public bool NewsExists(int id)
        {
            return _context.News.Any(n => n.Id == id);
        }
        public bool CreateNews(News news)
        {
            _context.Add(news);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateNews(News news)
        {
            _context.Update(news);

            return Save();
        }

        public bool DeleteNews(News news)
        {
            _context.Remove(news);

            return Save();
        }
    }
}
