using EMarket.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EMarket.Utility
{
    public static class UserExtension
    {
        public static IdentityUser ToIdentityUser(this RegisterCredentials user)
        {
            return new IdentityUser
            {
                UserName = user.Username,
                Email = user.Email
            };
        }

        public static async Task<string> GetUserIdAsync(this UserManager<IdentityUser> userManager, ClaimsPrincipal principal)
        {
            IdentityUser? user = await userManager.GetUserAsync(principal);
            return user is not null ? user.Id : string.Empty;
        }
    }
}
