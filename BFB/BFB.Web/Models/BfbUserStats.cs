using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbUserStats
    {
        public int UserStatId { get; set; }
        public int UserId { get; set; }
        public int GamesPlayed { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InsertedBy { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual BfbUser User { get; set; }
    }
}
