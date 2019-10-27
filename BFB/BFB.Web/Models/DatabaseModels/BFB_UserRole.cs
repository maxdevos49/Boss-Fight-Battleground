using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_UserRole
    {
        [Key]
        public int UserRoleId { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key
        public int RoleId { get; set; } // Foreign Key
        public Boolean IsActive { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime InsertedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime UpdatedBy { get; set; }
    }
}
