using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface IAuthorRepository
    {
        List<Author> GetAuthors();
        Author GetAuthor(int id);
        List<Book> GetBooksByAuthor(int authorId);
        bool AuthorExists(int id);
        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool Save();
    }
}
