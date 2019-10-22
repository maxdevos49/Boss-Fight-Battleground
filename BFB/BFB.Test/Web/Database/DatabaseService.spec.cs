using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFB.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;
using Moq;
using DatabaseConfig = BFB.Test.Web.Resources.DatabaseConfig;
using Moq.EntityFrameworkCore;

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

            IList<BFB_User> users = new List<BFB_User>();
            users.Add(fakeUser);
            DbContextOptions options = new DbContextOptions<DbContext>();
            var mockedDatabaseConfig = new Mock<BFB.Web.Models.DatabaseConfig>(options);
            mockedDatabaseConfig.Setup(foo => foo.BFB_User).ReturnsDbSet(users);
            BFB.Web.Services.DatabaseService.db = mockedDatabaseConfig.Object;


            BFB.Web.Services.DatabaseService.AddUserEntry(username, email, password);
            Mock.Verify();
        }

        [Fact]
        public void ValidateUserTest()
        {
            string username = "test";
            string password = "test2";
            string email = "test@test.test";
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
            IList<BFB_User> users = new List<BFB_User>();
            users.Add(fakeUser);
            DbContextOptions options = new DbContextOptions<DbContext>();
            var mockedDatabaseConfig = new Mock<BFB.Web.Models.DatabaseConfig>(options);
            mockedDatabaseConfig.Setup(foo => foo.BFB_User).ReturnsDbSet(users);
            BFB.Web.Services.DatabaseService.db = mockedDatabaseConfig.Object;

            Assert.True(BFB.Web.Services.DatabaseService.ValidateUser(username, password));
        }

        #endregion
    }
}
