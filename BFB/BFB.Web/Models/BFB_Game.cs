using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_Game
    {
        public int GameId { get; set; } // Primary key
        public int PlayerKills { get; set; }
        public int MonsterKills { get; set; }
        public int BossKills { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime InsertedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime UpdatedBy { get; set; }
    }
}
