using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using BFB.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BFB.Web.Services
{
    public static class DatabaseService
    {
        public static DatabaseConfig db { get; set; }

        // Add a user to BFB_User table
        public static void AddUserEntry(string username, string email, string password, DateTime currentTime)
        {
            string currentDate = currentTime.ToString("yyyy-MM-dd");
            db.BFB_User.Add(new BFB_User
            {
                Username = username,
                Email = email,
                Password = password,
                IsVerified = false,
                IsBanned = false,
                IsActive = false,
                InsertedOn = currentDate,
                UpdatedOn = currentDate,
                UpdatedBy = currentDate,
                EmailToken = null
            });
            db.SaveChanges();
        }

        public static Boolean ValidateUser(string username, string password)
        {
            var user = db.BFB_User
                .Where(u => u.Username == username && u.Password == password)
                .ToList();

            return (user.Count == 1);
        }
    }
}
