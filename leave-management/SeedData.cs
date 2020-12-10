using System;
using leave_management.Data;
using Microsoft.AspNetCore.Identity;

namespace leave_management
{
    public static class SeedData
    {
        public static void Seed(
            UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager
        ) {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(
            UserManager<Employee> userManager
        )
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new Employee { UserName = "admin@localhost", Email = "admin@localhost" };
                var result = userManager.CreateAsync(user, "P@ssword1").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private static void SeedRoles(
            RoleManager<IdentityRole> roleManager
        )
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole { Name = "Administrator" };
                roleManager.CreateAsync(role);
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole { Name = "Employee" };
                roleManager.CreateAsync(role);
            }
        }
    }
}
