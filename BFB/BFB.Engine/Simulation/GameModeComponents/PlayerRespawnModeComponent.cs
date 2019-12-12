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
            entity.Socket?.Emit("PlayerUIRequest", new DataMessage
            {
                Message = "MonsterMenuUI"
            });
        }
    }
}
