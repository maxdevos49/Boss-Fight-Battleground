using BFB.Engine.TileMap;

namespace BFB.Engine.Entity.Components.Physics
{
    public interface IPhysicsComponent
    {
        void Update(ServerEntity serverEntity, Chunk[,] chunks);
    }
}