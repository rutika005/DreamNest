using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Aesthetica
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionRole = context.HttpContext.Session.GetString("UserRole");

            if (sessionRole != _role)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }

}
