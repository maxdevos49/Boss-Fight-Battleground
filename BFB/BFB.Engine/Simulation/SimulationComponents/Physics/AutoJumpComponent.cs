using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.SimulationComponents.Physics
{
    public class AutoJumpComponent : EntityComponent
    {
        public AutoJumpComponent() : base(false) { }

        
        public override bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            //Auto Jump
            if (tc.Side == CollisionSide.RightBorder || tc.Side == CollisionSide.LeftBorder)
            {
                if (tc.BottomBlockY != (int) tc.TilePosition.Y + 1 || simulation.World.GetBlock(tc.LeftBlockX, (int) tc.TilePosition.Y - 1) != WorldTile.Air)
                    return true;

                if (!entity.Grounded)
                    return true;
                
                entity.Position.Y -= simulation.World.WorldOptions.WorldScale + 7;
                return false;
            }

            return true;
        }

    }
}