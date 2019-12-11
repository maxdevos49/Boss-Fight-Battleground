using System;
using BFB.Engine.Entity;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class BossSpawnModeComponent : GameModeComponent
    {
        private int _timeToSpawn;
        private bool _hasPlagueOccured;
        private readonly Random _random;

        public BossSpawnModeComponent()
        {
            _hasPlagueOccured = false;
            _timeToSpawn = 20 * 60 * 5; //TPS * seconds in min * 5 minutes.  Should spawn a boss every 5 minutes.
            _random = new Random();
        }

        public override void Update(Simulation simulation)
        {
            if (!_hasPlagueOccured)
            {
                ReleaseRatsIntoEurope(simulation); 
                _hasPlagueOccured = true;
            }
            else
            {
                _timeToSpawn -= 1;
                
                if (_timeToSpawn > 0)
                    return;
                
                CreateBoss(simulation, null, true);
                _timeToSpawn = 20 * 60 * 5;
            }
        }
        
        #region Pick a player to kill

        // Spawn the plague in Europe.
        private void ReleaseRatsIntoEurope(Simulation simulation)
        {
            // Kill off a random human and spawn them as the boss.
            int who = _random.Next(simulation.GetPlayerEntities().Count); 
            while (simulation.GetPlayerEntities()[who].EntityConfiguration.EntityKey != "Human")
            {
                who = _random.Next(simulation.GetPlayerEntities().Count);
            }

            CreateBoss(simulation, simulation.GetPlayerEntities()[who], false);
        }
        
        #endregion

        #region CreateBoss
        
        private void CreateBoss(Simulation simulation, [CanBeNull] SimulationEntity target, bool fromMonsters)
        {
            // Loop through player monsters and pick a random one to make a monster.
            if (target == null)
            {
                int who = _random.Next(simulation.GetPlayerEntities().Count);
                
                if (!fromMonsters)
                {
                    while (simulation.GetPlayerEntities()[who].EntityConfiguration.EntityKey != "Human")
                        who = _random.Next(simulation.GetPlayerEntities().Count);
                }
                else
                {
                    while (simulation.GetPlayerEntities()[who].EntityConfiguration.EntityKey == "Human")
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
        
        #endregion
    }
}
