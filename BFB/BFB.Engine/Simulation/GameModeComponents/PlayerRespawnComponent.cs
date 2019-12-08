using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PlayerRespawnComponent : GameComponent
    {
        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityType != EntityType.Player || reason == EntityRemovalReason.Disconnect || reason == EntityRemovalReason.BossSpawn) return;

            // Select new screen.

            SimulationEntity player = SimulationEntity.SimulationEntityFactory("Skeleton", socket: entity.Socket);
            player.Position.X = entity.Position.X;
            player.Position.Y = entity.Position.Y;
            simulation.AddEntity(player);
        }
    }
}
