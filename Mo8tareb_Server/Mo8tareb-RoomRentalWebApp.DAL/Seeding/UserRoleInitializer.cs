using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Mo8tareb_RoomRentalWebApp.DAL.Constants;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Seeding
{
    public static class UserRoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            string[] roleNames = { Authorization.Admin, Authorization.Owner, Authorization.User };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // create Admin
            if (userManager.FindByEmailAsync(Authorization.AdminEmail).Result is null)
            {
                AppUser adminUser = new AppUser()
                {
                    Email = Authorization.AdminEmail,
                    UserName = Authorization.AdminPassword,
                    FirstName = "Mohaned",
                    LastName = "Saudi",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(adminUser, Authorization.AdminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, Authorization.Admin).Wait();
                }
            }

            // create Owner
            if (userManager.FindByEmailAsync(Authorization.OwnerEmail).Result is null)
            {
                AppUser ownerUser = new Owner()
                {
                    Email = Authorization.OwnerEmail,
                    UserName = Authorization.OwnerPassword,
                    FirstName = "Ahmed",
                    LastName = "Fathy",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Gender = Enums.Gender.Male,
                    PhoneNumber = "01004512030"
                };

                IdentityResult result = userManager.CreateAsync(ownerUser, Authorization.OwnerPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(ownerUser, Authorization.Owner).Wait();
                }
            }

            // create User
            if (userManager.FindByEmailAsync(Authorization.UserEmail).Result is null)
            {
                AppUser normalUser = new AppUser()
                {
                    Email = Authorization.UserEmail,
                    UserName = Authorization.UserPassword,
                    FirstName = "MagedKarHA",
                    LastName = "Saudi",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(normalUser, Authorization.UserPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(normalUser, Authorization.User).Wait();
                }
            }
        }
    }
}
