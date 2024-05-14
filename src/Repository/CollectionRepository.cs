using LibraryApp.Data;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Models;

namespace LibraryApp.Repository
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly DataContext _context;

        public CollectionRepository(DataContext context)
        {
            _context = context;
        }

        public bool CollectionExists(int id)
        {
            return _context.Collections.Any(c => c.Id == id);
        }

        public List<Book> GetBooksByCollection(int id)
        {
            return _context.CollectionsBooks.Where(cb => cb.CollectionId == id).Select(cb => cb.Book).ToList();
        }

        public List<Collection> GetCollectionByBook(int id)
        {
            return _context.CollectionsBooks.Where(cb => cb.BookId == id).Select(cb => cb.Collection).ToList();
        }

        public List<Collection> GetCollections()
        {
            return _context.Collections.ToList();
        }

        public Collection GetCollection(int id)
        {
            return _context.Collections.Where(c  => c.Id == id).FirstOrDefault();
        }

        public bool CreateCollection(Collection collection)
        {
            _context.Add(collection);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateCollection(Collection collection)
        {
            _context.Update(collection);

            return Save();
        }

        public bool DeleteCollection(Collection collection)
        {
            _context.Remove(collection);

            return Save();
        }
    }
}
