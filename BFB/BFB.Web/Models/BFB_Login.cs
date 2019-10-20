using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_Login
    {
        [Key]
        public int LoginId { get; set; } // Primary Key
        public int UserId { get; set; }
        public string Token { get; set; }
        public Boolean IsActive { get; set; }
        public string InsertedOn { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
