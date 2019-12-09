using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Inventory;
using BFB.Engine.Simulation.EntityComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class AIMobSpawnModeComponent : GameModeComponent
    {

        private int AIMonstersAlive;
        private int AIMonstersMaxSpawnAmount;
        private int timeToSpawn;
        private Random _random;

        public AIMobSpawnModeComponent() : base()
        {
            AIMonstersAlive = 0;
            AIMonstersMaxSpawnAmount = 20; //TODO
            timeToSpawn = 15;
            _random = new Random();
        }

        public override void Update(Simulation simulation)
        {
            timeToSpawn -= 1;
            if (timeToSpawn <= 0)
            {
                if (AIMonstersAlive < AIMonstersMaxSpawnAmount)
                {
                    // spawn 5 zombies on each human player.
                    foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                    {
                        if (entity.EntityConfiguration.EntityKey != "Human") continue;


                        for (int i = 0; i < 5; i++)
                        {
                            int spawnLocationX = (int)entity.Position.X + _random.Next(-500, 500);
                            int spawnLocationY = (int)entity.Position.Y - _random.Next(0,10);

                            Tuple<int, int> blockPos =
                                simulation.World.BlockLocationFromPixel(spawnLocationX, spawnLocationY);

                            if (blockPos != null && simulation.World.GetBlock(blockPos.Item1, blockPos.Item2) == WorldTile.Air)
                            {
                                // Actually spawn the zombie.
                                SimulationEntity mob = SimulationEntity.SimulationEntityFactory("Zombie");
                                mob.Position.X = spawnLocationX;
                                mob.Position.Y = spawnLocationY;

                                IItem sword = new Item("ZombieSword");
                                mob.AddComponent(new InventoryComponent(new List<IItem>(){ sword }));

                                mob.ControlState = new ControlState();

                                mob.AddComponent(new InputAI());
                                mob.AddComponent(new AnimatedHolding());
                                simulation.AddEntity(mob);


                                AIMonstersAlive += 1;
                            }
                        }
                    }
                }

                timeToSpawn = 15;
            }
        }

        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityType == EntityType.Mob)
            {
                AIMonstersAlive -= 1;
            }
        }
    }
}
