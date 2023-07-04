using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName ="Mohamed Hussien",
                    Email ="Mohamed_Hussien@gmail.com",
                    UserName ="Mohamed.Hussien",
                    PhoneNumber="01159520090"
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");

               
            }
        }
    }
}
