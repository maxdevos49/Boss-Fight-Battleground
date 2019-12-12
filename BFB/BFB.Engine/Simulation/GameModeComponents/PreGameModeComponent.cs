using System;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PreGameModeComponent : GameModeComponent
    {
        private bool _gameReady;
        private int _timeToStart;
        private bool _showingCountdown;

        #region  Init

        public override void Init(Simulation simulation, SimulationGameMode gameMode)
        {
            base.Init(simulation, gameMode);
            
            _gameReady = false;
            _showingCountdown = false;
            _timeToStart = 20 * 10;
        }
        
        #endregion

        #region Reset
        
        public override void Reset(Simulation simulation)
        {
            base.Reset(simulation);
            
            _gameReady = false;
            _showingCountdown = false;
            _timeToStart = 20 * 10;
        }
        
        #endregion
        
        public override void Update(Simulation simulation)
        {
            // check number of players. When number of players >= 5, start a countdown.
            // once countdown ends, game starts.
            if (simulation.ConnectedClients >= GameMode.RequiredPlayers)
                _gameReady = true;

            if (_gameReady)
            {
                _timeToStart -= 1;

                // Show the countdown.
                if (_timeToStart < 20 * 10)
                {
                    DataMessage message = new DataMessage
                    {
                        Message = "CountdownUI"
                    };

                    foreach (SimulationEntity entity in simulation.GetPlayerEntities().Where(entity => !_showingCountdown))
                        entity?.Socket?.Emit("PlayerUIRequest", message);

                    _showingCountdown = true;
                }
            }

            if (!_gameReady || _timeToStart > 0) 
                return;
            
            GameMode.SwitchGameState(GameState.InProgress);


            DataMessage message2 = new DataMessage
            {
                Message = "HudUI"
            };
            
            foreach (SimulationEntity entity in simulation.GetPlayerEntities())
            {
                if (entity?.Socket == null || entity.EntityType != EntityType.Player)
                    return;

                entity.Socket.Emit("PlayerUIRequest", message2);
            }
        }

        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityType != EntityType.Player || reason == EntityRemovalReason.Disconnect || reason == EntityRemovalReason.BossSpawn)
                return;

            // Just respawn them if the game hasn't started
            SimulationEntity player = SimulationEntity.SimulationEntityFactory("Human", entity.Socket);
            GameMode.RespawnEntity(player);
        }
    }
}
