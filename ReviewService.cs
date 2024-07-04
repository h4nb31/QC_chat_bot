using QualityControl.Models;

namespace QualityControl
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewListModel review);
    }
    public class ReviewService : IReviewService
    {
        private readonly ApplicationContext _db;

        public ReviewService(ApplicationContext context)
        {
            _db = context;
        }

        public async Task AddReviewAsync(ReviewListModel review)
        {
            _db.ReviewListModels.Add(review);
            await _db.SaveChangesAsync();
        }
    }
}
