using TalabatBLL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatDAL.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Manar",
                    Email="manar@gmail.com",
                    UserName= "manar@gmail.com",
                    Address=new Address
                    {
                        FirstName="Manar",
                        LastName="Ismail",
                        Street="10 st 9 Maadi",
                        City="Maadi",
                        State="Cairo",
                        ZipCode="12345"
                    }

                };
                await userManager.CreateAsync(user);
            }
        }
    }
}
