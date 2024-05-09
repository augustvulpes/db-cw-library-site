using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface ICollectionService
    {
        public List<CollectionDto> GetCollections();
        public CollectionDto GetCollection(int collectionId);
        public List<BookDto> GetBooksByCollectionId(int collectionId);
        public string CreateCollection(CollectionDto collectionCreate);
        public string UpdateCollection(int collectionId, CollectionDto collectionUpdate);
        public string DeleteCollection(int collectionId);
    }
}
