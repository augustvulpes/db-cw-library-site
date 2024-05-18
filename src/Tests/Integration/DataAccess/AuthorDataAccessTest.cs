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
    public class AuthorDataAccessTest
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        public readonly IAuthorRepository _authorRepository;

        public AuthorDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _authorRepository = new AuthorRepository(context);
            _mapper = mapper;
            _authorService = new AuthorService(_authorRepository, _mapper);
        }

        [Fact]
        public void PostGetDeleteAuthors()
        {
            var testAuthor = new AuthorDto { Id = 9999, Country = "TEST", Name = "TEST" };

            _authorService.CreateAuthor(testAuthor);

            var author = _authorService.GetAuthor(testAuthor.Id);

            _authorService.DeleteAuthor(testAuthor.Id);

            Assert.Equivalent(testAuthor, author);
        }

        [Fact]
        public void GetAuthors()
        {
            var testAuthor1 = new AuthorDto { Id = 1001, Country = "TEST1001", Name = "TEST1001" };
            var testAuthor2 = new AuthorDto { Id = 1002, Country = "TEST1002", Name = "TEST1002" };

            var authors = new List<AuthorDto> { testAuthor1, testAuthor2 };

            _authorService.CreateAuthor(testAuthor1);
            _authorService.CreateAuthor(testAuthor2);

            var resultAuthors = _authorService.GetAuthors();

            _authorService.DeleteAuthor(testAuthor1.Id);
            _authorService.DeleteAuthor(testAuthor2.Id);

            Assert.Equivalent(authors, resultAuthors);
        }
    }
}
