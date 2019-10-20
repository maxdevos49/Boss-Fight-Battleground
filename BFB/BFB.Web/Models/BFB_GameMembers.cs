using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_GameMembers
    {
        [Key]
        public int GameMemberId { get; set; } // Primary Key
        public int GameId { get; set; } // Foreign Key
        public int UserId { get; set; } // Foreign Key
        public int BossKills { get; set; }
        public int MonsterKills { get; set; }
        public int PlayerKills { get; set; }
        public int TimeAsBoss { get; set; }
        public int TimeAsMonster { get; set; }
        public int TimeAsPlayer { get; set; }
        public Boolean IsActive { get; set; }
        public string InsertedOn { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
