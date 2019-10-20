using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_Role
    {
        public int RoleId { get; set; } // Primary Key
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime InsertedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime UpdatedBy { get; set; }
    }
}
