using BFB.Engine.TileMap;

namespace BFB.Engine.Entity.PhysicsComponents
{
    public interface IPhysicsComponent
    {
        void Update(SimulationEntity simulationEntity, WorldManager world);
    }
}