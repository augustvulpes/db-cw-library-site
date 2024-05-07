using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface IBookService
    {
        public List<BookDto> GetBooks();
        public BookDto GetBook(int bookId);
        public string CreateBook(int authorId, BookDto bookCreate);
        public string UpdateBook(int bookId, BookDto bookUpdate);
        public string DeleteBook(int bookId);
        public string AddOwnership(int authorId, int bookId);
        public string AddIntoCollection(int collectionId, int bookId);
    }
}
