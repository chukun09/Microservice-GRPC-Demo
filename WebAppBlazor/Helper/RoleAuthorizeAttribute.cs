using Core.Entites;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppBlazor.Helper
{
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly List<string> UserRoles;
        public RoleAuthorizeAttribute(string roles)
        {
            UserRoles = string.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(",").ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserEntity)context.HttpContext.Items["User"];
            var roles = (List<string>)context.HttpContext.Items["Roles"];

            if (user == null || roles == null || roles.Count == 0 || !roles.All(x => UserRoles.Contains(x)))
            {
                // not logged in
                throw new CustomException("Unauthorized", 401);
            }
        }
    }
}
