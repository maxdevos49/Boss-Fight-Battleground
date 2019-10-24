using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbUserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }

        public virtual BfbRole Role { get; set; }
        public virtual BfbUser User { get; set; }
    }
}
