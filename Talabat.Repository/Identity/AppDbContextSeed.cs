using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppDbContextSeed
    {
        public static async Task SeedUser(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new ApplicationUser()
                {
                    DisplayName = "Sherif Asharf",
                    Email = "shikoashraf12345@gmail.com",
                    UserName = "shikoashraf12345",
                    PhoneNumber = "01234567891"
                };
                await userManager.CreateAsync(User);
            }

        }
    }
}
