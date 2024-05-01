using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IReviewRepository
    {
        Review GetReview(int id);
        bool ReviewExists(int id);
    }
}
