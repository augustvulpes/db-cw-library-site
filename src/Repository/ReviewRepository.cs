using LibraryApp.Data;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Models;

namespace LibraryApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public List<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }
        public bool CreateReview(Review review)
        {
            _context.Add(review);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);

            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);

            return Save();
        }
    }
}
