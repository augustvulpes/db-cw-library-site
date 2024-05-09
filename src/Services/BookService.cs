using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICollectionRepository collectionRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _collectionRepository = collectionRepository;
            _mapper = mapper;
        }

        public string AddIntoCollection(int collectionId, int bookId)
        {
            var book = _bookRepository.GetBook(bookId);

            if (book == null)
                throw new NotFoundException("Book doesn't exist");

            var collection = _collectionRepository.GetCollection(collectionId);

            if (collection == null)
                throw new NotFoundException("Collection doesn't exist");

            var bookInCollection = _collectionRepository.GetBooksByCollection(collectionId).Where(b => b.Id == bookId).FirstOrDefault();

            if (bookInCollection != null)
                throw new UnprocessableException("Book is already in this collection");

            if (!_bookRepository.AddIntoCollection(collectionId, bookId))
                throw new Exception();

            return "Successfully added";
        }

        public string AddOwnership(int authorId, int bookId)
        {
            var book = _bookRepository.GetBook(bookId);

            if (book == null)
                throw new NotFoundException("Book doesn't exist");

            var author = _authorRepository.GetAuthor(authorId);

            if (author == null)
                throw new NotFoundException("Author doesn't exist");

            var bookInOwnership = _authorRepository.GetBooksByAuthor(authorId).Where(b => b.Id == bookId).FirstOrDefault();

            if (bookInOwnership != null)
                throw new UnprocessableException("Ownership already exists");

            if (!_bookRepository.AddOwnership(authorId, bookId))
                throw new Exception();

            return "Successfully added";
        }

        public string CreateBook(int authorId, BookDto bookCreate)
        {
            if (bookCreate == null)
                throw new BadRequestException();

            var book = _bookRepository.GetBooks()
                .Where(b => b.Title.Trim().ToUpper() == bookCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (book != null)
                throw new UnprocessableException("Book already exists");

            var author = _authorRepository.GetAuthor(authorId);

            if (author == null)
                throw new NotFoundException("Author doesn't exist");

            var bookMap = _mapper.Map<Book>(bookCreate);

            if (!_bookRepository.CreateBook(authorId, bookMap))
                throw new Exception();

            return "Successfully created";
        }

        public string DeleteBook(int bookId)
        {
            var book = _bookRepository.GetBook(bookId);

            if (book == null)
                throw new NotFoundException("Book doesn't exist");

            if (!_bookRepository.DeleteBook(book))
                throw new Exception();

            return "Successfully deleted";
        }

        public BookDto GetBook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                throw new NotFoundException("Book not found");

            var book = _mapper.Map<BookDto>(_bookRepository.GetBook(bookId));

            return book;
        }

        public List<BookDto> GetBooks()
        {
            var books = _mapper.Map<List<BookDto>>(_bookRepository.GetBooks());

            return books;
        }

        public string UpdateBook(int bookId, BookDto bookUpdate)
        {
            if (bookUpdate == null || bookId != bookUpdate.Id)
                throw new BadRequestException();

            if (!_bookRepository.BookExists(bookId))
                throw new NotFoundException("Book doesn't exist");

            var bookMap = _mapper.Map<Book>(bookUpdate);

            if (!_bookRepository.UpdateBook(bookMap))
                throw new Exception();

            return "Successfully updated";
        }
    }
}
