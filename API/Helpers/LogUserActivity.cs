using API.Entities;
using API.Extentions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var  ResultContext=await next();
            if (!ResultContext.HttpContext.User.Identity.IsAuthenticated) return;
            var UserId = ResultContext.HttpContext.User.GetUserId();
            var repo = ResultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(UserId);
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}
