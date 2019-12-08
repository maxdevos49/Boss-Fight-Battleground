using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class BossSpawnComponent : GameComponent
    {
        private int BossesAlive;
        private int timeToSpawn;
        private bool hasPlagueOccured;
        private Random _random;

        public BossSpawnComponent() : base()
        {
            BossesAlive = 0;
            hasPlagueOccured = false;
            timeToSpawn = 20 * 60 * 5; //TPS * seconds in min * 5 minutes.  Should spawn a boss every 5 minutes.
            _random = new Random();
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
                    CreateBoss(simulation, null);
                    timeToSpawn = 20 * 60 * 5;
                }
            }
        }

        // Spawn the plague in Europe.
        private void ReleaseRatsIntoEurope(Simulation simulation)
        {
            // Kill off a random human and spawn them as the boss.
            int who = _random.Next(simulation.GetPlayerEntities().Count);
            while (simulation.GetPlayerEntities()[who].EntityConfiguration.EntityKey != "Human")
            {
                who = _random.Next(simulation.GetPlayerEntities().Count);
            }

            CreateBoss(simulation, simulation.GetPlayerEntities()[1]);
        }

        private void CreateBoss(Simulation simulation, [CanBeNull] SimulationEntity target)
        {
            // Loop through player monsters and pick a random one to make a monster.
            if (target == null)
            {
                int who = _random.Next(simulation.GetPlayerEntities().Count);
                while (simulation.GetPlayerEntities()[who].EntityConfiguration.EntityKey != "Human")
                {
                    who = _random.Next(simulation.GetPlayerEntities().Count);
                }

                target = simulation.GetPlayerEntities()[who];
            }

            simulation.RemoveEntity(target.EntityId, EntityRemovalReason.BossSpawn);

            SimulationEntity player = SimulationEntity.SimulationEntityFactory("AdamBoss", socket: target.Socket);
            player.Position.X = target.Position.X;
            player.Position.Y = target.Position.Y;
            simulation.AddEntity(player);
        }
    }
}
