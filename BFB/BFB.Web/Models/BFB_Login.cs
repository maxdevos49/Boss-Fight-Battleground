using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_Login
    {
        public int LoginId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime InsertedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime UpdatedBy { get; set; }
    }
}
