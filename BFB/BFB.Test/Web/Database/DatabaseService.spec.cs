using System;
using System.Collections.Generic;
using BFB.Web.Models;
using Xunit;
using Moq;
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

            BfbUser fakeUser = new BfbUser
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

            //Create context mock
            Mock<BFBContext> mockedDatabaseConfig = new Mock<BFBContext>();
            IList<BfbUser> users = new List<BfbUser>();

            //mock setup adding a user
            mockedDatabaseConfig.Setup(foo => foo.BfbUser.Add(fakeUser)).Verifiable();
            
            //mock setup getting a user
            mockedDatabaseConfig.Setup(foo => foo.BfbUser).ReturnsDbSet(users);

            //mock test add
            users.Add(fakeUser);
            
            //mock test Adding a new user
            BFB.Web.Services.DatabaseService.AddUserEntry(mockedDatabaseConfig.Object, username, email, password);
            
            //Verify if mock expectations worked
            Mock.Verify();
        }

        [Fact]
        public void ValidateUserTest()
        {
            string username = "test";
            string password = "test2";
            string email = "test@test.test";
            DateTime currentDate = DateTime.Now;

            BfbUser fakeUser = new BfbUser
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
            IList<BfbUser> users = new List<BfbUser>();
            users.Add(fakeUser);
            var mockedDatabaseConfig = new Mock<BFBContext>();
            mockedDatabaseConfig.Setup(foo => foo.BfbUser).ReturnsDbSet(users);

            Assert.True(BFB.Web.Services.DatabaseService.ValidateUser(mockedDatabaseConfig.Object,username, password));
        }

        #endregion
    }
}
