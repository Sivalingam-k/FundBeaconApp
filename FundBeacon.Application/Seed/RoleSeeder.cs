using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Application.Seed
{
    public class RoleSeeder
    {
        private static readonly List<string> Roles = new()
        {
            "ADMIN",
            "STUDENT",
            "SCHOLORSHIP-PROVIDER",
            "SUB-SCHOLORSHIP-PROVIDER"

        };
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role: {role}");
                    }
                }
            }
        }
    }
}
