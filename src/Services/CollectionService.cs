using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IMapper _mapper;

        public CollectionService(ICollectionRepository collectionRepository, IMapper mapper)
        {
            _collectionRepository = collectionRepository;
            _mapper = mapper;
        }

        public List<CollectionDto> GetCollections()
        {
            var collections = _mapper.Map<List<CollectionDto>>(_collectionRepository.GetCollections());

            return collections;
        }

        public CollectionDto GetCollection(int collectionId)
        {
            if (!_collectionRepository.CollectionExists(collectionId))
                throw new NotFoundException("Collection not found");

            var Collection = _mapper.Map<CollectionDto>(_collectionRepository.GetCollection(collectionId));

            return Collection;
        }

        public List<BookDto> GetBooksByCollectionId(int collectionId)
        {
            var books = _mapper.Map<List<BookDto>>(_collectionRepository.GetBooksByCollection(collectionId));

            return books;
        }

        public string CreateCollection(CollectionDto collectionCreate)
        {
            if (collectionCreate == null)
                throw new BadRequestException();

            var collection = _collectionRepository.GetCollections()
                .Where(a => a.Title.Trim().ToUpper() == collectionCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (collection != null)
                throw new UnprocessableException("Collection already exists");

            var collectionMap = _mapper.Map<Collection>(collectionCreate);

            if (!_collectionRepository.CreateCollection(collectionMap))
                throw new Exception();

            return "Successfully created";
        }

        public string UpdateCollection(int collectionId, CollectionDto collectionUpdate)
        {
            if (collectionUpdate == null || collectionId != collectionUpdate.Id)
                throw new BadRequestException();

            if (!_collectionRepository.CollectionExists(collectionId))
                throw new NotFoundException("Collection not found");

            var collectionMap = _mapper.Map<Collection>(collectionUpdate);

            if (!_collectionRepository.UpdateCollection(collectionMap))
                throw new Exception();

            return "Successfully updated";
        }

        public string DeleteCollection(int collectionId)
        {
            var collection = _collectionRepository.GetCollection(collectionId);

            if (collection == null)
                throw new NotFoundException("Collection not found");

            if (!_collectionRepository.DeleteCollection(collection))
                throw new Exception();

            return "Successfully deleted";
        }
    }
}
