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
            if (simulation.GetPlayerEntities().Count == 0)
            {
                ResetGame(simulation);
            }

            timeToRestart -= 1;
            if (timeToRestart <= 0)
            {
                ConvertAllPlayersToHumans(simulation);

                ResetGame(simulation);
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

        private void ResetGame(Simulation simulation)
        {
            simulation.gameComponents.Clear();
            simulation.gameComponents.Add(new PreGameComponent());
            simulation.gameState = GameState.PreGame;
        }
    }
}
