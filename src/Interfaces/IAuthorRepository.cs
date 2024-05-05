using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int id);
        ICollection<Book> GetBooksByAuthor(int authorId);
        bool AuthorExists(int id);
        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool Save();
    }
}
