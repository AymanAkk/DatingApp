using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using DatingApp.API.Models.Data;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public  class SeedUsers
    {
        public static void Seedusers(DataContext context)
        {
           if(!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach(var user in users)
                {
                    byte[] passwordHash,passwordSalt;
                    string password= "Password";
                    CreatePasswordHash(password,out passwordHash,out passwordSalt);
                    user.PasswordHash=passwordHash;
                    user.PasswordSalt=passwordSalt;
                    user.UserName=user.UserName.ToLower();
                    context.Add(user);
                }
                context.SaveChanges();
            }
        }

         private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
              passwordSalt=hmac.Key;
              passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

    }
}