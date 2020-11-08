using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            // we first check if our db already contain any user, if so we do not want to seed d db
            if (await context.Users.AnyAsync()) return;

            //if we av no user, we get the content of our UserSeedData and store it in a variable
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            // we convert the json data into a list of AppUser
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            //Now we create a user for each object in the list
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                //we using hardcoded value to compute the password since its seed data not real data
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123999abc"));
                user.PasswordSalt = hmac.Key;
                 context.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
