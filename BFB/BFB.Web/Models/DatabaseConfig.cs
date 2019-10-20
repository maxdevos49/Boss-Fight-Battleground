﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BFB.Web.Models
{
    public class DatabaseConfig : DbContext
    {
        public string ConnectionString { get; set; }

        public DbSet<BFB_User> Users { get; set; }
        public DbSet<BFB_UserRole> UserRoles { get; set; }
        public DbSet<BFB_UserStats> UserStats { get; set; }
        public DbSet<BFB_Role> Roles { get; set; }
        public DbSet<BFB_Login> Logins { get; set; }
        public DbSet<BFB_Game> Games { get; set; }
        public DbSet<BFB_GameMembers> GameMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(ConnectionString);
    }
}
