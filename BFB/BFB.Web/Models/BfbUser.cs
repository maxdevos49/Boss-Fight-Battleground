using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbUser
    {
        public BfbUser()
        {
            BfbGameMembers = new HashSet<BfbGameMembers>();
            BfbLogin = new HashSet<BfbLogin>();
            BfbUserRole = new HashSet<BfbUserRole>();
            BfbUserStats = new HashSet<BfbUserStats>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public sbyte IsVerified { get; set; }
        public sbyte IsBanned { get; set; }
        public sbyte IsActive { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }
        public string EmailToken { get; set; }

        public virtual ICollection<BfbGameMembers> BfbGameMembers { get; set; }
        public virtual ICollection<BfbLogin> BfbLogin { get; set; }
        public virtual ICollection<BfbUserRole> BfbUserRole { get; set; }
        public virtual ICollection<BfbUserStats> BfbUserStats { get; set; }
    }
}
