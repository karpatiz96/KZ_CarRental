using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface IRatingService
    {
        Task PostRating(int ratingValue, int vehicleModelId, int currentUserId);

        Task<bool> IsRated(int vehicleModelId, int currentUserId);

        Task DeleteUserRatings(int userId);
    }
}
