using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PostGameModeComponent : GameModeComponent
    {
        private int timeToRestart;
<<<<<<< HEAD:BFB/BFB.Engine/Simulation/GameModeComponents/PostGameModeComponent.cs
        public PostGameModeComponent() : base()
=======
        private bool showingCountdown;
        public PostGameComponent() : base()
>>>>>>> Fix respawning in pre and post game so you don't break stuff:BFB/BFB.Engine/Simulation/GameModeComponents/PostGameComponent.cs
        {
            Console.WriteLine("GAME ENDED");
            timeToRestart = 20 * 10;
            showingCountdown = false;
        }

        public override void Update(Simulation simulation)
        {
            if (simulation.GetPlayerEntities().Count == 0)
            {
                ResetGame(simulation);
            }

            timeToRestart -= 1;

            // Start the countdown to restart.
            if (timeToRestart < 20 * 8) {

                    DataMessage message = new DataMessage
                    {
                        Message = "GameOverUI"
                    };

                    DataMessage textMessage = new DataMessage
                    {
                        Message = "Returning to pregame in " + timeToRestart + "..."
                    };

                    foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                    {
                        if (entity == null || entity.Socket == null || entity.EntityType != EntityType.Player)
                            return;

                        if (!showingCountdown)
                            entity.Socket.Emit("PlayerUIRequest", message);

                        entity.Socket.Emit("onUpdateDisplay", textMessage);
                    }
                    showingCountdown = true;
            }

            if (timeToRestart <= 0)
            {
                ConvertAllPlayersToHumans(simulation);

                ResetGame(simulation);

                DataMessage message = new DataMessage();
                message.Message = "HudUI";
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

            simulation.AddEntity(player);
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
            simulation.GameComponents.Clear();
            simulation.GameComponents.Add(new PreGameModeComponent());
            simulation.GameState = GameState.PreGame;
        }
    }
}
