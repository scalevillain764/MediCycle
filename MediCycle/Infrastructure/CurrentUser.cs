using Domain;
using System.Security.Claims;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor? _accessor;
        public CurrentUser(IHttpContextAccessor? accessor) => _accessor = accessor;
        public Ulid? UserId
        {
            get
            {
                var context = _accessor.HttpContext;

                if (context == null)
                    return null;

                return Ulid.TryParse(
                    context.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    out var userId)
                        ? userId
                        : null;
            }
        }

        public string? UserType
        {
            get
            {
                var context = _accessor.HttpContext;

                if (context == null) return null;

                return context.User.FindFirstValue(ClaimTypes.Role);
            }
        }
    }
}