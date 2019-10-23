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

        #region BFB_User
        // Add a user to BFB_User table
        public static void AddUserEntry(string username, string email, string password)
        {
            DateTime currentDate = DateTime.Now;
            db.BFB_User.Add(new BfbUser
            {
                Username = username,
                Email = email,
                Password = password,
                IsVerified = 0,
                IsBanned = 0,
                IsActive = 0,
                InsertedOn = currentDate,
                UpdatedOn = currentDate,
                UpdatedBy = currentDate,
                EmailToken = null
            });
            db.SaveChanges();
        }

        // Check if a user with the given username and password exists in the database.
        public static Boolean ValidateUser(string username, string password)
        {
            var user = db.BFB_User
                .Where(u => u.Username == username && u.Password == password)
                .ToList();

            return (user.Count == 1);
        }
        #endregion
    }


}
