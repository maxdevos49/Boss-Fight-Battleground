using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_User
    {
        [Key]
        public int UserId { get; set; } // Primary Key
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailToken { get; set; }
        public Boolean IsVerified { get; set; }
        public Boolean IsBanned { get; set; }
        public Boolean IsActive { get; set; }
        public string InsertedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
