using CarRental.Bll.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetComments();

        CommentDto PostComment(int vehicleModelId, string text, int currentUserId);

        CommentDto DeleteComment(int commentId, int currentUserId);
        
    }
}
