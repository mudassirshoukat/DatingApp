﻿using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,RoleManager<AppRole> roleManageer)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userdata = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users=JsonSerializer.Deserialize<List<AppUser>>(userdata, options);

            var roles = new List<AppRole>
            {
                new AppRole{Name="Member"},
                new AppRole{Name="Admin"},
                new AppRole{Name="Moderator"},
            };

            foreach (var role in roles)
            {
                var rolesresult = await roleManageer.CreateAsync(role);
            }

            foreach (var user in users)
            {
                //using var hmac = new HMACSHA512();
                // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$woD"));
                // user.PasswordSalt = hmac.Key;
          
                 var userresult=   await userManager.CreateAsync(user, "password");
                await userManager.AddToRoleAsync(user, "Member");
            }


            var admin = new AppUser
            {
                UserName = "admin"
            };
            var adminresult = await userManager.CreateAsync(admin, "adminpassword");
            var adminroleresult = await userManager.AddToRolesAsync(admin, new[] {"Admin","Moderator"});
            
        }
    }
}
