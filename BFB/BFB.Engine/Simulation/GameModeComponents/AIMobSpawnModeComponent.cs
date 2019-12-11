using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Inventory;
using BFB.Engine.Simulation.EntityComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class AIMobSpawnModeComponent : GameModeComponent
    {

        private int _aiMonstersAlive;
        private readonly int _aiMonstersMaxSpawnAmount;
        private int _timeToSpawn;
        private readonly Random _random;

        public AIMobSpawnModeComponent()
        {
            _aiMonstersAlive = 0;
            _aiMonstersMaxSpawnAmount = 10;
            _timeToSpawn = 45;
            _random = new Random();
        }

        public override void Update(Simulation simulation)
        {
            _timeToSpawn -= 1;
            
            if (_timeToSpawn > 0)
                return;


            if (_aiMonstersAlive > _aiMonstersMaxSpawnAmount)
                return;

            // spawn 5 zombies on each human player.
            foreach (SimulationEntity entity in simulation.GetPlayerEntities())
            {
                if (entity.EntityConfiguration.EntityKey != "Human") continue;


                for (int i = 0; i < 5; i++)
                {
                    int spawnLocationX = (int)entity.Position.X + _random.Next(-1000, 1000);
                    int spawnLocationY = (int)entity.Position.Y - _random.Next(50,1000);

                    Tuple<int, int> blockPos = simulation.World.BlockLocationFromPixel(spawnLocationX, spawnLocationY);

                    if (blockPos == null || simulation.World.GetBlock(blockPos.Item1, blockPos.Item2) != WorldTile.Air) 
                        continue;
                    
                    // Actually spawn the zombie.
                    SimulationEntity mob = SimulationEntity.SimulationEntityFactory("Zombie");
                    
                    mob.ControlState = new ControlState();
                    mob.Position.X = spawnLocationX;
                    mob.Position.Y = spawnLocationY;

                    IItem sword = new Item("ZombieSword");
                    
                    mob.AddComponent(new InventoryComponent(new List<IItem>(){ sword }));
                    mob.AddComponent(new InputAI());
                    mob.AddComponent(new AnimatedHolding());

                    simulation.AddEntity(mob);

                    _aiMonstersAlive += 1;
                }
            }

            _timeToSpawn = 45;
        }

        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityType == EntityType.Mob)
            {
                _aiMonstersAlive -= 1;
            }
        }
    }
}
