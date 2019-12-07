using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class BossSpawnComponent : GameComponent
    {
        private int BossesAlive;
        private int timeToSpawn;
        private bool hasPlagueOccured;

        public BossSpawnComponent() : base()
        {
            BossesAlive = 0;
            hasPlagueOccured = false;
            timeToSpawn = 20 * 60 * 5; //TPS * seconds in min * 5 minutes.  Should spawn a boss every 5 minutes.
        }

        public override void Update(Simulation simulation)
        {
            if (!hasPlagueOccured)
            {
                ReleaseRatsIntoEurope(simulation); 
                hasPlagueOccured = true;
            }
            else
            {
                timeToSpawn -= 1;
                if (timeToSpawn <= 0)
                {
                    CreateBossFromMonsters();
                    timeToSpawn = 20 * 60 * 5;
                }
            }
        }

        // Spawn the plague in Europe.
        private void ReleaseRatsIntoEurope(Simulation simulation)
        {
            // Kill off a random human and spawn them as the boss.
        }

        private void CreateBossFromMonsters()
        {
            // Loop through player monsters and pick a random one to make a monster.
        }
    }
}
