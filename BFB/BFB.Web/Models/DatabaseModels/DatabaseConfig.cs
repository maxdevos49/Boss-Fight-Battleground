using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BFB.Web.Models
{
    public class DatabaseConfig : DbContext
    {
        public DatabaseConfig(DbContextOptions opt) : base(opt) { }

        public string ConnectionString { get; set; }

        public DbSet<BFB_User> BFB_User { get; set; }
        public DbSet<BFB_UserRole> BFB_UserRole { get; set; }
        public DbSet<BFB_UserStats> BFB_UserStats { get; set; }
        public DbSet<BFB_Role> BFB_Role { get; set; }
        public DbSet<BFB_Login> BFB_Login { get; set; }
        public DbSet<BFB_Game> BFB_Game { get; set; }
        public DbSet<BFB_GameMembers> BFB_GameMembers { get; set; }

        
    }
}
