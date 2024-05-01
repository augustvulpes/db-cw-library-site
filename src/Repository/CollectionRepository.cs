using LibraryApp.Data;
using LibraryApp.Interfaces;
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

        public ICollection<Book> GetBooksByCollection(int id)
        {
            return _context.CollectionsBooks.Where(cb => cb.CollectionId == id).Select(cb => cb.Book).ToList();
        }

        public ICollection<Collection> GetCollectionByBook(int id)
        {
            return _context.CollectionsBooks.Where(cb => cb.BookId == id).Select(cb => cb.Collection).ToList();
        }

        public ICollection<Collection> GetCollections()
        {
            return _context.Collections.ToList();
        }

        public Collection GetCollection(int id)
        {
            return _context.Collections.Where(c  => c.Id == id).FirstOrDefault();
        }
    }
}
