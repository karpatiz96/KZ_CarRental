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

        public IContentModeratorService _contentModeratorService { get; set; }

        public CommentService(CarRentalDbContext dbContext, IContentModeratorService contentModeratorService)
        {
            _dbContext = dbContext;
            _contentModeratorService = contentModeratorService;
        }

        public static Expression<Func<Comment, CommentDto>> CommentDtoSelector { get; } = c => new CommentDto
        {
            Id = c.Id,
            Text = c.Text,
            CreationDate = c.CreationDate,
            UserId = c.UserId,
            UserName = c.User.UserName ?? "",
            Name = c.User.Name ?? "",
            VehicleModelId = c.VehicleModelId
        };

        public async Task<IEnumerable<CommentDto>> GetComments()
        {
            return await _dbContext.Comments
                .OrderByDescending(c => c.CreationDate)
                .Select(CommentDtoSelector)
                .ToListAsync();
        }

        public CommentDto PostComment(int vehicleModelId, string text, int currentUserId)
        {
            //var moderatedText = _contentModeratorService.ModerateText(text);

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

            var comment = _dbContext.Comments.Where(c => c.Id == commentId).SingleOrDefault();

            _dbContext.Comments.Remove(comment);

            _dbContext.SaveChanges();

            return commentDto;
        }
    }
}
