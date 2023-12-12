using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimPrincipalExtension
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse( user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

    }
}
