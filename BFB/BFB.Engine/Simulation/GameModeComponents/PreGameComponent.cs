using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PreGameComponent : GameComponent
    {
        private bool gameReady;
        private int timeToStart;
        private bool gameStarted;

        public PreGameComponent() : base()
        {
            gameReady = false;
            gameStarted = false;
            timeToStart = 10;
        }

        public override void Update(Simulation simulation)
        {
            if (gameStarted) return;
            // check number of players. When number of players >= 5, start a countdown.
            // once countdown ends, game starts.
            if (simulation.ConnectedClients >= 2)
                gameReady = true;

            if (gameReady)
            {
                timeToStart -= 1;
            }

            if (gameReady && timeToStart <= 0)
            {
                simulation.gameComponents.Add(new ManaRegenComponent());
                simulation.gameComponents.Add(new AIMobSpawnComponent());
                simulation.gameComponents.Add(new BossSpawnComponent());
                simulation.gameComponents.Add(new PlayerRespawnComponent());
                
                GameComponent component = new GameEndComponent();
                component.Init(simulation);
                simulation.gameComponents.Add(component);

                simulation.gameState = GameState.InProgress;

                Console.WriteLine("GAME STARTED");
                gameStarted = true;
            }
        }
    }
}
