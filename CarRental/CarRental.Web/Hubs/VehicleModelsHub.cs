using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Dal.Entities;
using CarRental.Web.ViewRender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.Hubs
{
    public class VehicleModelsHub : Hub<IHubClient>
    {
        public ICommentService _commentService { get; set; }
        public UserManager<User> UserManager { get; set; }
        public IViewRender ViewRender { get; set; }

        public VehicleModelsHub(ICommentService commentService, UserManager<User> userManager, IViewRender viewRender)
        {
            _commentService = commentService;
            UserManager = userManager;
            ViewRender = viewRender;
        }

        private int? currentUserId;

        public int? CurrentUserId => Context.User.Identity.IsAuthenticated ?
            (currentUserId ?? (currentUserId = int.Parse(UserManager.GetUserId(Context.User)))) : null;

        public async Task JoinVehicleModelPage(int id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"vehiclemodel-{id}");
        }

        public async Task PostComment(int vehicleModelId, string text)
        {
            var comment = await _commentService.PostComment(vehicleModelId, text, CurrentUserId.Value);

            var htmlString = ViewRender.Render<CommentDto>("_CommentPartial", comment, false);

            await Clients.Groups($"vehiclemodel-{vehicleModelId}").CommentPosted(htmlString);
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = _commentService.DeleteComment(commentId, CurrentUserId.Value);
            await Clients.Groups($"vehiclemodel-{comment.VehicleModelId}").CommentDeleted(commentId);
        }

    }

    public interface IHubClient
    {
        Task CommentPosted(string htmlString);
        Task CommentDeleted(int commentId);
    }
}
