using Microsoft.AspNetCore.Authorization;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Webapi.Services
{
    public class PlaylistRoleRequirement : IAuthorizationRequirement
    {
        public PlaylistRole Role { get; }
        public int PlaylistId { get; }

        public PlaylistRoleRequirement(PlaylistRole role)
        {
            Role = role;
        }
    }
    public class PlaylistRoleHandler : AuthorizationHandler<PlaylistRoleRequirement>
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlaylistRoleHandler(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PlaylistRoleRequirement requirement)
        {
            if (_httpContextAccessor.HttpContext == null) return Task.CompletedTask;
            var playlistId = _httpContextAccessor.HttpContext.Request.RouteValues["playlistId"];
            if (playlistId != null)
            {
                int playlistIdValue;
                if (int.TryParse(playlistId.ToString(), out playlistIdValue))
                {
                    var userEmail = context.User?.Identity?.Name;
                    if (string.IsNullOrEmpty(userEmail))
                    {
                        return Task.CompletedTask;
                    }

                    var role = _context.AccountPlaylists.FirstOrDefault(a => a.Account.Email == userEmail && a.PlaylistId == playlistIdValue)?.Role;
                    if (role == requirement.Role)
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }

    }
}
