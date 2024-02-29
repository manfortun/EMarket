﻿using EMarket.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EMarket.Utility
{
    public static class UserExtension
    {
        /// <summary>
        /// Creates <c>IdentityUser</c> instance from <paramref name="user"/>.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IdentityUser ToIdentityUser(this RegisterCredentials user)
        {
            return new IdentityUser
            {
                UserName = user.Username,
                Email = user.Email
            };
        }

        /// <summary>
        /// Obtains the user ID of a user asynchronously
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static async Task<string> GetUserIdAsync(this UserManager<IdentityUser> userManager, ClaimsPrincipal principal)
        {
            IdentityUser? user = await userManager.GetUserAsync(principal);
            return user is not null ? user.Id : string.Empty;
        }
    }
}
