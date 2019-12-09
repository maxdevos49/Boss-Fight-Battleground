using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PreGameModeComponent : GameModeComponent
    {
        private bool gameReady;
        private int timeToStart;
        private bool gameStarted;
        private bool showingCountdown;

        public PreGameModeComponent() : base()
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
                if (timeToStart < 20 * 8)
                {
                    DataMessage message = new DataMessage
                    {
                        Message = "CountdownUI"
                    };

                    DataMessage textMessage = new DataMessage
                    {
                        Message = "Starting in " + timeToStart + "..."
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
            }

            if (gameReady && timeToStart <= 0)
            {
                simulation.GameComponents.Add(new ManaRegenModeComponent());
                simulation.GameComponents.Add(new AIMobSpawnModeComponent());
                simulation.GameComponents.Add(new BossSpawnModeComponent());
                simulation.GameComponents.Add(new PlayerRespawnModeComponent());
                
                GameModeComponent component = new GameModeEndComponent();
                component.Init(simulation);
                simulation.GameComponents.Add(component);

                simulation.GameState = GameState.InProgress;

                Console.WriteLine("GAME STARTED");
                gameStarted = true;

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
            if (gameStarted)
                return;

            if (entity.EntityType != EntityType.Player || reason == EntityRemovalReason.Disconnect || reason == EntityRemovalReason.BossSpawn)
                return;

            // Just respawn them if the game hasn't started
            SimulationEntity player = SimulationEntity.SimulationEntityFactory("Human", entity.Socket);

            simulation.AddEntity(player);
        }
    }
}
