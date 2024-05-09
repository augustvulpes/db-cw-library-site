using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface IReviewService
    {
        public List<ReviewDto> GetReviews();
        public ReviewDto GetReview(int reviewId);
        public string CreateReview(ReviewDto reviewCreate);
        public string UpdateReview(int reviewId, ReviewDto reviewUpdate);
        public string DeleteReview(int reviewId);
    }
}
