using LibraryApp.Data;
using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;
        public BookRepository(DataContext context)
        {
            _context = context;
        }

        public bool BookExists(int id)
        {
            return _context.Books.Any(b => b.Id == id);
        }

        public Book GetBook(int id)
        {
            return _context.Books.Where(b => b.Id == id).FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return _context.Books.OrderBy(b => b.Id).ToList();
        }

        public ICollection<Book> GetBooks(string title)
        {
            return _context.Books.Where(b => b.Title.Contains(title)).OrderBy(b => b.Id).ToList();
        }
        public bool CreateBook(Book book)
        {
            _context.Add(book);

            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }
    }
}
