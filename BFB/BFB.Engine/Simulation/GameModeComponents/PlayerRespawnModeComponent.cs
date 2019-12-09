using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PlayerRespawnModeComponent : GameModeComponent
    {
        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityType != EntityType.Player || reason == EntityRemovalReason.Disconnect || reason == EntityRemovalReason.BossSpawn) return;

            // Select new screen.
            DataMessage message = new DataMessage();
            message.Message = "MonsterMenuUI";
            entity.Socket.Emit("PlayerUIRequest", message);
        }
    }
}
