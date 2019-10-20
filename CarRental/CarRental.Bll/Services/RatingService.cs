using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class RatingService : IRatingService
    {
        public CarRentalDbContext _dbContext { get; }

        public RatingService(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task PostRating(int ratingValue, int vehicleModelId, int currentUserId)
        {
            var rating = new Rating
            {
                UserId = currentUserId,
                Value = ratingValue,
                VehicleModelId = vehicleModelId
            };

            _dbContext.Ratings.Add(rating);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsRated(int vehicleModelId, int currentUserId)
        {
            var isRated = await _dbContext.Ratings
                .Where(v => v.VehicleModelId == vehicleModelId && v.UserId == currentUserId)
                .AnyAsync();

            return isRated;
        }

        public async Task DeleteUserRatings(int userId)
        {
            var ratings = await _dbContext.Ratings
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var user = await _dbContext.Users
                .Include(u => u.Ratings)
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            foreach (var item in ratings)
            {
                item.User = null;
                user.Ratings.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
