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

namespace LibraryApp.Tests.Integration.DataAccess
{
    public class CollectionDataAccessTest
    {
        private readonly ICollectionService _collectionService;
        private readonly IMapper _mapper;
        public readonly ICollectionRepository _collectionRepository;

        public CollectionDataAccessTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost; Database=library; Username=postgres; Password=adminadmin")
                .Options;

            var context = new DataContext(options);

            _collectionRepository = new CollectionRepository(context);
            _mapper = mapper;
            _collectionService = new CollectionService(_collectionRepository, _mapper);
        }

        [Fact]
        public void PostGetDeleteCollections()
        {
            var testCollection = new CollectionDto { Id = 9999, Title="TEST", CreationDate= DateTime.UtcNow, Description="TEST" };

            _collectionService.CreateCollection(testCollection);

            var collection = _collectionService.GetCollection(testCollection.Id);

            _collectionService.DeleteCollection(testCollection.Id);

            Assert.Equivalent(testCollection, collection);
        }

        [Fact]
        public void GetCollections()
        {
            var testCollection1 = new CollectionDto { Id = 9990, Title = "TEST1", CreationDate = DateTime.UtcNow, Description = "TEST" };
            var testCollection2 = new CollectionDto { Id = 9991, Title = "TEST2", CreationDate = DateTime.UtcNow, Description = "TEST" };

            var collections = new List<CollectionDto> { testCollection1, testCollection2 };

            _collectionService.CreateCollection(testCollection1);
            _collectionService.CreateCollection(testCollection2);

            var resultCollections = _collectionService.GetCollections();

            _collectionService.DeleteCollection(testCollection1.Id);
            _collectionService.DeleteCollection(testCollection2.Id);

            Assert.Equivalent(collections, resultCollections);
        }
    }
}
