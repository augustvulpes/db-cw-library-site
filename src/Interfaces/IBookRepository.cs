using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        ICollection<Book> GetBooks(string title);
        Book GetBook(int id);
        bool BookExists(int id);
        bool CreateBook(Book book);
        bool Save();
    }
}
