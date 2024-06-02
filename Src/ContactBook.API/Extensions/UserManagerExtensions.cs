using ContactBook.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ContactBook.API.Extensions
{
    public static class UserManagerExtensions
    {
        /// <summary>
        /// An extension to access the current profile
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<AppUser> FindUserByClaimPrincipalWithProfile(this UserManager<AppUser>
            userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.Include(x => x.Profile).SingleOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// An extension to access the current user
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<AppUser> FindEmailByClaimPrincipal(this UserManager<AppUser>
            userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// An extension to access the current userId
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static AppUser FindUserId(this UserManager<AppUser>
            userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var User = userManager.Users.SingleOrDefaultAsync(x => x.Email == email)?.Result;
            return User;
        }
    }
}
