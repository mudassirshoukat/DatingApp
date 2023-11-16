using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userdata = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users=JsonSerializer.Deserialize<List<AppUser>>(userdata, options);

            foreach (var user in users)
            {
               using var hmac = new HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$woD"));
                user.PasswordSalt = hmac.Key;
               await context.Users.AddAsync(user);

            }
            await context.SaveChangesAsync();
            
            
        }
    }
}
