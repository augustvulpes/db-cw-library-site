using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface IReviewRepository
    {
        List<Review> GetReviews();
        Review GetReview(int id);
        bool ReviewExists(int id);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();
    }
}
