using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbGameMembers
    {
        public int GameMemberId { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int BossKills { get; set; }
        public int MonsterKills { get; set; }
        public int PlayerKills { get; set; }
        public int TimeAsBoss { get; set; }
        public int TimeAsMonster { get; set; }
        public int TimeAsPlayer { get; set; }
        public sbyte? IsActive { get; set; }
        public DateTime? InsertedBy { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }

        public virtual BfbGame Game { get; set; }
        public virtual BfbUser User { get; set; }
    }
}
