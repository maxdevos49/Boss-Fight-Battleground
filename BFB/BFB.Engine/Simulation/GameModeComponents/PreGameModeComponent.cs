using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PreGameComponent : GameComponent
    {
        private bool _gameReady;
        private int _timeToStart;
        private bool _gameStarted;
        private bool _showingCountdown;

        public PreGameComponent() : base()
        {
            _gameReady = false;
            _gameStarted = false;
            _showingCountdown = false;
            _timeToStart = 20 * 10;
        }

        public override void Update(Simulation simulation)
        {
            if (_gameStarted) 
                return;
            
            // check number of players. When number of players >= 5, start a countdown.
            // once countdown ends, game starts.
            if (simulation.ConnectedClients >= 2)
                _gameReady = true;

            if (_gameReady)
            {
                _timeToStart -= 1;
                
                // Show the countdown.
                DataMessage message = new DataMessage
                {
                    Message = "CountdownUI"
                };

                foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                {
                    entity.Socket?.Emit("PlayerUIRequest", message);
                    //entity.Socket.Emit("onUpdateDisplay", textMessage);
                }
                _showingCountdown = true;
            }

            if (_gameReady && _timeToStart <= 0)
            {
                simulation.GameComponents.Add(new ManaRegenComponent());
                simulation.GameComponents.Add(new AIMobSpawnComponent());
                simulation.GameComponents.Add(new BossSpawnComponent());
                simulation.GameComponents.Add(new PlayerRespawnComponent());
                
                GameComponent component = new GameEndComponent();
                component.Init(simulation);
                simulation.GameComponents.Add(component);

                simulation.GameState = GameState.InProgress;

                Console.WriteLine("GAME STARTED");
                _gameStarted = true;

                DataMessage message = new DataMessage
                {
                    Message = "HudUI"
                };
                foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                {
                    entity.Socket?.Emit("PlayerUIRequest", message);
                }
            }
        }
    }
}
