using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class CommentService : ICommentService
    {
        public CarRentalDbContext _dbContext { get; }

        public CommentService(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<Comment, CommentDto>> CommentDtoSelector { get; } = c => new CommentDto
        {
            Id = c.Id,
            Text = c.Text,
            CreationDate = c.CreationDate,
            UserId = c.UserId ?? 0,
            User = c.User.UserName ?? "",
            VehicleModelId = c.VehicleModelId
        };

        public IEnumerable<CommentDto> GetComments()
        {
            return _dbContext.Comments.OrderByDescending(c => c.CreationDate).Select(CommentDtoSelector).ToList();
        }

        public CommentDto PostComment(int vehicleModelId, string text, int currentUserId)
        {
            var comment = new Comment
            {
                Text = text,
                CreationDate = DateTimeOffset.Now,
                UserId = currentUserId,
                VehicleModelId = vehicleModelId
            };

            _dbContext.Comments.Add(comment);

            _dbContext.SaveChanges();

            return _dbContext.Comments.Where(c => c.Id == comment.Id).Select(CommentDtoSelector).SingleOrDefault();
        }

        public CommentDto DeleteComment(int commentId, int currentUserId)
        {
            var commentDto = _dbContext.Comments
                .Where(c => c.Id == commentId)
                .Select(CommentDtoSelector)
                .SingleOrDefault();

            var comment = _dbContext.Comments.Where(c => c.Id == commentId && c.UserId == currentUserId).SingleOrDefault();

            _dbContext.Comments.Remove(comment);

            _dbContext.SaveChanges();

            return commentDto;
        }
    }
}
