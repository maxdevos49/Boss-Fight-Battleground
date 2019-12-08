using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PreGameComponent : GameComponent
    {
        private bool gameReady;
        private int timeToStart;
        private bool gameStarted;
        private bool showingCountdown;

        public PreGameComponent() : base()
        {
            gameReady = false;
            gameStarted = false;
            showingCountdown = false;
            timeToStart = 20 * 10;
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
                
                // Show the countdown.
                DataMessage message = new DataMessage();
                message.Message = "CountdownUI";

                DataMessage textMessage = new DataMessage();
                textMessage.Message = "Starting in " + timeToStart + "...";
                foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                {
                    entity.Socket.Emit("PlayerUIRequest", message);
                    //entity.Socket.Emit("onUpdateDisplay", textMessage);
                }
                showingCountdown = true;
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

                DataMessage message = new DataMessage();
                message.Message = "HudUI";
                foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                {
                    entity.Socket.Emit("PlayerUIRequest", message);
                }
            }
        }
    }
}
