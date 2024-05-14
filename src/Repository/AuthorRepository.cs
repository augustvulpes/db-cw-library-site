using LibraryApp.Data;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Models;

namespace LibraryApp.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;
        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public bool AuthorExists(int id)
        {
            return _context.Authors.Any(a => a.Id == id);
        }

        public Author GetAuthor(int id)
        {
            return _context.Authors.Where(a => a.Id == id).FirstOrDefault();
        }

        public List<Author> GetAuthors()
        {
            return _context.Authors.OrderBy(a => a.Id).ToList();
        }

        public List<Book> GetBooksByAuthor(int authorId)
        {
            return _context.AuthorsBooks.Where(ab => ab.AuthorId == authorId).Select(ab => ab.Book).ToList();
        }

        public bool CreateAuthor(Author author)
        {
            _context.Add(author);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateAuthor(Author author)
        {
            _context.Update(author);

            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _context.Remove(author);

            return Save();
        }
    }
}
