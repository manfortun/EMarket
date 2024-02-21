using EMarket.Models;
using Microsoft.AspNetCore.Identity;

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
    }
}
