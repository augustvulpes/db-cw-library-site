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
    public class AuthorServiceUnitTests
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        public readonly Mock<IAuthorRepository> _authorRepositoryMock = new();

        public AuthorServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = config.CreateMapper();

            _authorService = new AuthorService(_authorRepositoryMock.Object, mapper);
            _mapper = mapper;
        }

        [Fact]
        public void GetAuthors()
        {
            var testAuthors = new List<Author> 
            { 
                new Author { Id = 1, Country = "Russia", Name = "A. S. Pushkin" },
                new Author { Id = 2, Country = "Russia", Name = "L. N. Tolstoy" },
            };

            _authorRepositoryMock.Setup(r => r.GetAuthors()).Returns(testAuthors);

            var resultAuthors = _mapper.Map<List<Author>>(_authorService.GetAuthors());

            Assert.Equivalent(testAuthors, resultAuthors);
        }

        [Fact]
        public void GetAuthor()
        {
            var testAuthor = new Author { Id = 1, Country = "Russia", Name = "A. S. Pushkin" };

            _authorRepositoryMock.Setup(r => r.AuthorExists(It.IsAny<int>())).Returns(true);
            _authorRepositoryMock.Setup(r => r.GetAuthor(It.IsAny<int>())).Returns(testAuthor);

            var resultAuthor = _mapper.Map<Author>(_authorService.GetAuthor(1));

            Assert.Equivalent(testAuthor, resultAuthor);
        }

        [Fact]
        public void GetBooksByAuthorId()
        {
            var testBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Abc", Pages = 128, Year = 2000, BBK = "asd" }
            };

            _authorRepositoryMock.Setup(r => r.GetBooksByAuthor(It.IsAny<int>())).Returns(testBooks);

            var resultBooks = _mapper.Map<List<Book>>(_authorService.GetBooksByAuthorId(1));

            Assert.Equivalent(testBooks, resultBooks);
        }

        [Fact]
        public void CreateAuthor()
        {
            var authorCreate = new AuthorDto { Id = 1, Country = "Russia", Name = "A. S. Pushkin" };

            _authorRepositoryMock.Setup(r => r.GetAuthors()).Returns(new List<Author> { });
            _authorRepositoryMock.Setup(r => r.CreateAuthor(It.IsAny<Author>())).Returns(true);

            var result = _authorService.CreateAuthor(authorCreate);

            Assert.Equivalent("Successfully created", result);
        }

        [Fact]
        public void UpdateAuthor()
        {
            var authorUpdate = new AuthorDto { Id = 1, Country = "Russia", Name = "A. S. Pushkin" };

            _authorRepositoryMock.Setup(r => r.AuthorExists(It.IsAny<int>())).Returns(true);
            _authorRepositoryMock.Setup(r => r.UpdateAuthor(It.IsAny<Author>())).Returns(true);

            var result = _authorService.UpdateAuthor(1, authorUpdate);

            Assert.Equivalent("Successfully updated", result);
        }

        [Fact]
        public void DeleteAuthor()
        {
            var testAuthor = new Author { Id = 1, Country = "Russia", Name = "A. S. Pushkin" };

            _authorRepositoryMock.Setup(r => r.GetAuthor(It.IsAny<int>())).Returns(testAuthor);
            _authorRepositoryMock.Setup(r => r.DeleteAuthor(testAuthor)).Returns(true);

            var result = _authorService.DeleteAuthor(1);

            Assert.Equivalent("Successfully deleted", result);
        }
    }
}
