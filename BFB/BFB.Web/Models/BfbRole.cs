using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbRole
    {
        public BfbRole()
        {
            BfbUserRole = new HashSet<BfbUserRole>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }

        public virtual ICollection<BfbUserRole> BfbUserRole { get; set; }
    }
}
