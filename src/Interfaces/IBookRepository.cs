using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        ICollection<Book> GetBooks(string title);
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
