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
    public class CollectionServiceUnitTests
    {
        private readonly ICollectionService _collectionService;
        private readonly IMapper _mapper;
        public readonly Mock<ICollectionRepository> _collectionRepositoryMock = new();

        public CollectionServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = config.CreateMapper();

            _collectionService = new CollectionService(_collectionRepositoryMock.Object, mapper);
            _mapper = mapper;
        }

        [Fact]
        public void GetCollections()
        {
            var testCollections = new List<Collection>
            {
                new Collection { Id = 1, Description = "test", Title = "Abc", CreationDate = DateTime.Today },
                new Collection { Id = 2, Description = "test2", Title = "Abc", CreationDate = DateTime.Today }
            };

            _collectionRepositoryMock.Setup(r => r.GetCollections()).Returns(testCollections);

            var resultCollections = _mapper.Map<List<Collection>>(_collectionService.GetCollections());

            Assert.Equivalent(testCollections, resultCollections);
        }

        [Fact]
        public void GetCollection()
        {
            var testCollection = new Collection { Id = 1, Description = "test", Title = "Abc", CreationDate = DateTime.Today };

            _collectionRepositoryMock.Setup(r => r.CollectionExists(It.IsAny<int>())).Returns(true);
            _collectionRepositoryMock.Setup(r => r.GetCollection(It.IsAny<int>())).Returns(testCollection);

            var resultCollection = _mapper.Map<Collection>(_collectionService.GetCollection(1));

            Assert.Equivalent(testCollection, resultCollection);
        }

        [Fact]
        public void GetBooksByCollectionId()
        {
            var testBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Abc", Pages = 128, Year = 2000, BBK = "asd" }
            };

            _collectionRepositoryMock.Setup(r => r.GetBooksByCollection(It.IsAny<int>())).Returns(testBooks);

            var resultBooks = _mapper.Map<List<Book>>(_collectionService.GetBooksByCollectionId(1));

            Assert.Equivalent(testBooks, resultBooks);
        }

        [Fact]
        public void CreateCollection()
        {
            var collectionCreate = new CollectionDto { Id = 1, Description = "test", Title = "Abc", CreationDate = DateTime.Today };

            _collectionRepositoryMock.Setup(r => r.GetCollections()).Returns(new List<Collection> { });
            _collectionRepositoryMock.Setup(r => r.CreateCollection(It.IsAny<Collection>())).Returns(true);

            var result = _collectionService.CreateCollection(collectionCreate);

            Assert.Equivalent("Successfully created", result);
        }

        [Fact]
        public void UpdateCollection()
        {
            var collectionUpdate = new CollectionDto { Id = 1, Description = "test", Title = "Abc", CreationDate = DateTime.Today };

            _collectionRepositoryMock.Setup(r => r.CollectionExists(It.IsAny<int>())).Returns(true);
            _collectionRepositoryMock.Setup(r => r.UpdateCollection(It.IsAny<Collection>())).Returns(true);

            var result = _collectionService.UpdateCollection(1, collectionUpdate);

            Assert.Equivalent("Successfully updated", result);
        }

        [Fact]
        public void DeleteCollection()
        {
            var testCollection = new Collection { Id = 1, Description = "test", Title = "Abc", CreationDate = DateTime.Today };

            _collectionRepositoryMock.Setup(r => r.GetCollection(It.IsAny<int>())).Returns(testCollection);
            _collectionRepositoryMock.Setup(r => r.DeleteCollection(testCollection)).Returns(true);

            var result = _collectionService.DeleteCollection(1);

            Assert.Equivalent("Successfully deleted", result);
        }
    }
}
