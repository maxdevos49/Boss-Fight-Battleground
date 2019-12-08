using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PostGameComponent : GameComponent
    {
        private int timeToRestart;
        public PostGameComponent() : base()
        {
            Console.WriteLine("GAME ENDED");
            timeToRestart = 20 * 10;
        }

        public override void Update(Simulation simulation)
        {
            timeToRestart -= 1;
            if (timeToRestart <= 0)
            {
                ConvertAllPlayersToHumans(simulation);

                simulation.gameComponents.Clear();
                simulation.gameComponents.Add(new PreGameComponent());

                Console.WriteLine("Restarting Game...");
                simulation.gameState = GameState.PreGame;
            }
        }

        private void ConvertAllPlayersToHumans(Simulation simulation)
        {
            foreach (SimulationEntity entity in simulation.GetPlayerEntities())
            {
                simulation.RemoveEntity(entity.EntityId);
                SimulationEntity player = SimulationEntity.SimulationEntityFactory("Human", entity.Socket);
                simulation.AddEntity(player);
            }
        }
    }
}
