using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using InvestmentSimulatorAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentSimulatorAPI.Attributes
{
    public class AdminOnlyAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var repository = context.HttpContext.RequestServices.GetService<AuthRepository>();
            if (repository == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                context.Result = new UnauthorizedResult(); 
                return;
            }

            var user = await repository.GetUserById(userId);
            if (user == null || !user.IsAdmin)
            {
                context.Result = new ForbidResult();
                return;
            }
        }        
    }   
}