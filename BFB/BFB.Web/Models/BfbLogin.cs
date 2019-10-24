using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbLogin
    {
        public int LoginId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }

        public virtual BfbUser User { get; set; }
    }
}
