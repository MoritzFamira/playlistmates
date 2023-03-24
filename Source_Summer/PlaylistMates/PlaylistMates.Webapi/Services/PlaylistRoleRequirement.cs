using Microsoft.AspNetCore.Authorization;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Webapi.Services
{
    public class PlaylistRoleRequirement : IAuthorizationRequirement
    {
        public List<PlaylistRole> Roles { get; }
        public int PlaylistId { get; }

        public PlaylistRoleRequirement(List<PlaylistRole> roles)
        {
            Roles = roles;
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
                    Console.WriteLine("Read DB");
                    // TODO: Instead of calling DB, create singleton cache
                    var role = _context.AccountPlaylists.FirstOrDefault(a => a.Account.Email == userEmail && a.PlaylistId == playlistIdValue)?.Role;
                    foreach (var item in requirement.Roles)
                    {
                        if (role == item)
                        {
                            context.Succeed(requirement);
                        }
                    }

                }
            }

            return Task.CompletedTask;
        }

    }
}
