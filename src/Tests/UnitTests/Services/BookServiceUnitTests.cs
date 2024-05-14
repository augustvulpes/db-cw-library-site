using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Interfaces.RepositoryInterfaces;
using Xunit;
using Moq;
using LibraryApp.Models;
using LibraryApp.Services;
using AutoMapper;
using LibraryApp.Helper;
using LibraryApp.Dto;

namespace LibraryApp.Tests.UnitTests.Services
{
    public class BookServiceUnitTests
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public readonly Mock<IBookRepository> _bookRepositoryMock = new();
        public readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
        public readonly Mock<ICollectionRepository> _collectionRepositoryMock = new();

        public BookServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = config.CreateMapper();

            _bookService = new BookService(_bookRepositoryMock.Object, 
                _authorRepositoryMock.Object,
                _collectionRepositoryMock.Object,
                mapper);
            _mapper = mapper;
        }

        [Fact]
        public void AddIntoCollection()
        {
            var newBook = new Book { Id = 2, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };
            var testCollection = new Collection { Id = 1, Description = "asd", Title = "qwe"};

            _collectionRepositoryMock.Setup(r => r.GetCollection(It.IsAny<int>())).Returns(testCollection);
            _collectionRepositoryMock.Setup(r => r.GetBooksByCollection(It.IsAny<int>())).Returns(new List<Book> { });
            _bookRepositoryMock.Setup(r => r.GetBook(It.IsAny<int>())).Returns(newBook);
            _bookRepositoryMock.Setup(r => r.AddIntoCollection(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            
            var result = _bookService.AddIntoCollection(1, 1);

            Assert.Equivalent("Successfully added", result);
        }

        [Fact]
        public void AddOwnership()
        {
            var newBook = new Book { Id = 2, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };
            var testAuthor = new Author { Id = 1, Country = "Russia", Name = "A. S. Pushkin" };

            _authorRepositoryMock.Setup(r => r.GetAuthor(It.IsAny<int>())).Returns(testAuthor);
            _authorRepositoryMock.Setup(r => r.GetBooksByAuthor(It.IsAny<int>())).Returns(new List<Book> { });
            _bookRepositoryMock.Setup(r => r.GetBook(It.IsAny<int>())).Returns(newBook);
            _bookRepositoryMock.Setup(r => r.AddOwnership(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = _bookService.AddOwnership(1, 1);

            Assert.Equivalent("Successfully added", result);
        }

        [Fact]
        public void CreateBook()
        {
            var bookCreate = new BookDto { Id = 1, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };
            var testAuthor = new Author { Id = 1, Country = "Russia", Name = "A. S. Pushkin" };

            _bookRepositoryMock.Setup(r => r.GetBooks()).Returns(new List<Book> { });
            _bookRepositoryMock.Setup(r => r.CreateBook(It.IsAny<int>(), It.IsAny<Book>())).Returns(true);
            _authorRepositoryMock.Setup(r => r.GetAuthor(It.IsAny<int>())).Returns(testAuthor);

            var result = _bookService.CreateBook(1, bookCreate);

            Assert.Equivalent("Successfully created", result);
        }

        [Fact]
        public void DeleteBook()
        {
            var bookDelete = new Book { Id = 1, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };

            _bookRepositoryMock.Setup(r => r.GetBook(It.IsAny<int>())).Returns(bookDelete);
            _bookRepositoryMock.Setup(r => r.DeleteBook(It.IsAny<Book>())).Returns(true);

            var result = _bookService.DeleteBook(1);

            Assert.Equivalent("Successfully deleted", result);
        }

        [Fact]
        public void GetBook()
        {
            var testBook = new Book { Id = 1, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };

            _bookRepositoryMock.Setup(r => r.BookExists(It.IsAny<int>())).Returns(true);
            _bookRepositoryMock.Setup(r => r.GetBook(It.IsAny<int>())).Returns(testBook);

            var resultBook = _mapper.Map<Book>(_bookService.GetBook(1));

            Assert.Equivalent(testBook, resultBook);
        }

        [Fact]
        public void GetBooks()
        {
            var testBooks = new List<Book>
            {
                new Book { Id = 2, Title = "Qweasd", Pages = 128, Year = 2000, BBK = "asd" },
                new Book { Id = 1, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" }
            };

            _bookRepositoryMock.Setup(r => r.GetBooks()).Returns(testBooks);

            var resultBooks = _mapper.Map<List<Book>>(_bookService.GetBooks());

            Assert.Equivalent(testBooks, resultBooks);
        }

        [Fact]
        public void UpdateBook()
        {
            var bookUpdate = new BookDto { Id = 1, Title = "Qwe", Pages = 128, Year = 2000, BBK = "asd" };

            _bookRepositoryMock.Setup(r => r.BookExists(It.IsAny<int>())).Returns(true);
            _bookRepositoryMock.Setup(r => r.UpdateBook(It.IsAny<Book>())).Returns(true);

            var result = _bookService.UpdateBook(1, bookUpdate);

            Assert.Equivalent("Successfully updated", result);
        }
    }
}
