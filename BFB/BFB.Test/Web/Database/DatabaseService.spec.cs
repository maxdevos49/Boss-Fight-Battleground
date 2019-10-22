using System;
using System.Collections.Generic;
using System.Text;
using BFB.Web.Models;
using Xunit;
using Moq;
using DatabaseConfig = BFB.Test.Web.Resources.DatabaseConfig;

namespace BFB.Test.Web.Database
{
    public class DatabaseService
    {
        #region BFB_User

        [Fact]
        public void AddUserEntryTest()
        {
            string username = "test";
            string password = "test";
            string email = "test@test.org";
            DateTime currentDate = DateTime.Now;

            BFB_User fakeUser = new BFB_User
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
            };

            var mock = new Mock<DatabaseConfig>();
            mock.Setup(foo => foo.BFB_User.Add(fakeUser)).Verifiable();
            //BFB.Web.Services.DatabaseService.AddUserEntry(username, email, password);
        }

        #endregion
    }
}
