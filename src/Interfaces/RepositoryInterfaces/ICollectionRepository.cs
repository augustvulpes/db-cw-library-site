using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface ICollectionRepository
    {
        List<Collection> GetCollections();
        Collection GetCollection(int id);
        List<Book> GetBooksByCollection(int collectionId);
        List<Collection> GetCollectionByBook(int id);
        bool CollectionExists(int id);
        bool CreateCollection(Collection collection);
        bool UpdateCollection(Collection collection);
        bool DeleteCollection(Collection collection);
        bool Save();
    }
}
