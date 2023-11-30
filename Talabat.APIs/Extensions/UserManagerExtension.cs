using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtension
    {
        public async static Task<ApplicationUser> GetUserWithAddressAsync(this UserManager<ApplicationUser> userManager,ClaimsPrincipal user)
        {
            var Email = user.FindFirstValue(ClaimTypes.Email);
            var User =await userManager.Users.Where(U => U.Email == Email).Include(U=>U.Address).FirstOrDefaultAsync();
            return User;
        }
    }
}
