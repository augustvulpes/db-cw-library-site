using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface ICollectionRepository
    {
        ICollection<Collection> GetCollections();
        Collection GetCollection(int id);
        ICollection<Book> GetBooksByCollection(int collectionId);
        ICollection<Collection> GetCollectionByBook(int id);
        bool CollectionExists(int id);
        bool CreateCollection(Collection collection);
        bool UpdateCollection(Collection collection);
        bool DeleteCollection(Collection collection);
        bool Save();
    }
}
