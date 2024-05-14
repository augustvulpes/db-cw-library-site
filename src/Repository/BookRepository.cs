using LibraryApp.Data;
using LibraryApp.Interfaces.RepositoryInterfaces;
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

        public List<Book> GetBooks()
        {
            return _context.Books.OrderBy(b => b.Id).ToList();
        }

        public List<Book> GetBooks(string title)
        {
            return _context.Books.Where(b => b.Title.Contains(title)).OrderBy(b => b.Id).ToList();
        }

        public bool CreateBook(int authorId, Book book)
        {
            var authorEntity = _context.Authors.Where(a => a.Id == authorId).FirstOrDefault();

            var authorBook = new AuthorBook()
            {
                Author = authorEntity,
                Book = book,
            };

            _context.Add(authorBook);
            _context.Add(book);

            return Save();
        }

        public bool AddOwnership(int authorId, int bookId)
        {
            var authorEntity = _context.Authors.Where(a => a.Id == authorId).FirstOrDefault();
            var bookEntity = _context.Books.Where(b => b.Id == bookId).FirstOrDefault();

            var authorBook = new AuthorBook()
            {
                Author = authorEntity,
                Book = bookEntity,
            };

            _context.Add(authorBook);

            return Save();
        }

        public bool AddIntoCollection(int collectionId, int bookId)
        {
            var collectionEntity = _context.Collections.Where(c => c.Id == collectionId).FirstOrDefault();
            var bookEntity = _context.Books.Where(b => b.Id == bookId).FirstOrDefault();

            var collectionBook = new CollectionBook()
            {
                Collection = collectionEntity,
                Book = bookEntity,
            };

            _context.Add(collectionBook);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateBook(Book book)
        {
            _context.Update(book);

            return Save();
        }

        public bool DeleteBook(Book book)
        {
            _context.Remove(book);

            return Save();
        }
    }
}
