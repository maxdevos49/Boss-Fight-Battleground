using System;
using System.Collections.Generic;

namespace BFB.Web.Models
{
    public partial class BfbGame
    {
        public BfbGame()
        {
            BfbGameMembers = new HashSet<BfbGameMembers>();
        }

        public int GameId { get; set; }
        public int PlayerKills { get; set; }
        public int MonsterKills { get; set; }
        public int BossKills { get; set; }
        public DateTime? InsertedOn { get; set; }
        public DateTime? InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }

        public virtual ICollection<BfbGameMembers> BfbGameMembers { get; set; }
    }
}
