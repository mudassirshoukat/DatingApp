using API.Entities;
using API.Extentions;
using API.Interfaces.RepoInterfaces;
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
            var unitOfWork = ResultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(UserId);
            user.LastActive = DateTime.UtcNow;
            await unitOfWork.Complete();
        }
    }
}
