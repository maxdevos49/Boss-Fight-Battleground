using System;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PostGameModeComponent : GameModeComponent
    {
        private int _timeToRestart;
        private bool _showingCountdown;

        public PostGameModeComponent()
        {
            _timeToRestart = 20 * 10;
            _showingCountdown = false;
        }

        public override void Update(Simulation simulation)
        {
            if (simulation.GetPlayerEntities().Count == 0)
            {
                ResetGame(simulation);
            }

            _timeToRestart -= 1;

            // Start the countdown to restart.
            if (_timeToRestart < 20 * 8) {

                    DataMessage message = new DataMessage
                    {
                        Message = "GameOverUI"
                    };

                    DataMessage textMessage = new DataMessage
                    {
                        Message = "Returning to pregame in " + _timeToRestart + "..."
                    };

                    foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                    {
                        if (entity == null || entity.Socket == null || entity.EntityType != EntityType.Player)
                            return;

                        if (!_showingCountdown)
                            entity.Socket.Emit("PlayerUIRequest", message);

                        entity.Socket.Emit("onUpdateDisplay", textMessage);
                    }
                    _showingCountdown = true;
            }

            if (_timeToRestart <= 0)
            {
                ConvertAllPlayersToHumans(simulation);

                ResetGame(simulation);

                DataMessage message = new DataMessage
                {
                    Message = "HudUI"
                };
                foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                {
                    if (entity == null || entity.Socket == null || entity.EntityType != EntityType.Player)
                        return;

                    entity.Socket.Emit("PlayerUIRequest", message);
                }
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

        private void ConvertAllPlayersToHumans(Simulation simulation)
        {
            foreach (SimulationEntity entity in simulation.GetPlayerEntities())
            {
                simulation.RemoveEntity(entity.EntityId);
                SimulationEntity player = SimulationEntity.SimulationEntityFactory("Human", entity.Socket);
                GameMode.RespawnEntity(player);
            }
        }

        private void ResetGame(Simulation simulation)
        {
            GameMode.SwitchGameState(GameState.PreGame);
//            simulation.GameComponents.Clear();
//            simulation.GameComponents.Add(new PreGameModeComponent());//TODO Change game state
//            simulation.GameState = GameState.PreGame;
        }
    }
}
