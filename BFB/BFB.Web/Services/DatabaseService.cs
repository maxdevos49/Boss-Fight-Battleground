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

        #region BFB_User
        // Add a user to BFB_User table
        public static void AddUserEntry(BFBContext db, string username, string email, string password)
        {
            DateTime currentDate = DateTime.Now;
            db.BfbUser.Add(new BfbUser
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

        // Check if a user with the given username and password exists in the database.
        public static bool ValidateUser(BFBContext db, string username, string password)
        {
            List<BfbUser> user = db.BfbUser
                .Where(u => u.Username == username && u.Password == password)
                .ToList();

            return user.Count == 1;
        }
        #endregion
    }


}
