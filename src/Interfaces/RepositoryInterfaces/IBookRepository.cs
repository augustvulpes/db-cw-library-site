using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface IBookRepository
    {
        List<Book> GetBooks();
        List<Book> GetBooks(string title);
        Book GetBook(int id);
        bool BookExists(int id);
        bool CreateBook(int authorId, Book book);
        bool AddOwnership(int authorId, int bookId);
        bool AddIntoCollection(int collectionId, int bookId);
        bool UpdateBook(Book book);
        bool DeleteBook(Book book);
        bool Save();
    }
}
