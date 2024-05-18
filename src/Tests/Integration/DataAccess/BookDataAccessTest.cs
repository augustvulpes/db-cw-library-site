using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Interfaces.RepositoryInterfaces;
using Xunit;
using LibraryApp.Services;
using AutoMapper;
using LibraryApp.Helper;
using LibraryApp.Dto;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Repository;

namespace LibraryApp.Tests.UnitTests.Services
{
    public class BookDataAccessTest
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        public readonly IBookRepository _bookRepository;
        public readonly IAuthorRepository _authorRepository;
        public readonly ICollectionRepository _collectionRepository;

        public BookDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _bookRepository = new BookRepository(context);
            _authorRepository = new AuthorRepository(context);
            _collectionRepository = new CollectionRepository(context);
            _mapper = mapper;

            _authorService = new AuthorService(_authorRepository, _mapper);
            _bookService = new BookService(_bookRepository,
                _authorRepository,
                _collectionRepository,
                _mapper);
        }

        [Fact]
        public void PostGetDeleteBooks()
        {
            var testAuthor = new AuthorDto { Id = 9876, Country = "TEST", Name = "TEST001" };

            _authorService.CreateAuthor(testAuthor);

            var testBook = new BookDto { Id = 9999, Year = 2000, BBK="aaa", Pages=42, Title="TEST" };

            _bookService.CreateBook(testAuthor.Id, testBook);

            var book = _bookService.GetBook(testBook.Id);

            _bookService.DeleteBook(testBook.Id);
            _authorService.DeleteAuthor(testAuthor.Id);

            Assert.Equivalent(testBook, book);
        }

        [Fact]
        public void GetBooks()
        {
            var testAuthor = new AuthorDto { Id = 1230, Country = "TEST", Name = "TEST002" };

            _authorService.CreateAuthor(testAuthor);

            var testBook1 = new BookDto { Id = 9990, Year = 2000, BBK = "aaa", Pages = 42, Title = "TEST1" };
            var testBook2 = new BookDto { Id = 9991, Year = 2000, BBK = "aaa", Pages = 42, Title = "TEST2" };

            var books = new List<BookDto> { testBook1, testBook2 };

            _bookService.CreateBook(testAuthor.Id, testBook1);
            _bookService.CreateBook(testAuthor.Id, testBook2);

            var resultBooks = _bookService.GetBooks();

            _bookService.DeleteBook(testBook1.Id);
            _bookService.DeleteBook(testBook2.Id);
            _authorService.DeleteAuthor(testAuthor.Id);

            Assert.Equivalent(books, resultBooks);
        }
    }
}
