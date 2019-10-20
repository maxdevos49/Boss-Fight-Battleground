using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFB.Web.Models
{
    public class BFB_Game
    {
        [Key]
        public int GameId { get; set; } // Primary key
        public int PlayerKills { get; set; }
        public int MonsterKills { get; set; }
        public int BossKills { get; set; }
        public string InsertedOn { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
