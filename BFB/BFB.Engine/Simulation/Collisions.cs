using System;
using System.Drawing;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation
{
    public static class Collision
    {
        
        #region DetectCollision

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulation"></param>
        /// <param name="entity"></param>
        /// <param name="physics"></param>
        public static void DetectCollision(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity)
        {
            
            physics.WorldBoundaryCheck(simulation, entity);


            if (physics.CollideWithFilters.Contains("tile"))
            {
                //Left and right collisions
                physics.BroadPhaseHorizontal(simulation, entity);

                //Detect up and down
                physics.BroadPhaseVertical(simulation, entity);
            }
            
            
        }

        #endregion

        #region WorldBoundryCollisions

        private static void WorldBoundaryCheck(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity)
        {
            //World Boundaries
            if (entity.Position.X < 0)
            {
                if (physics.OnWorldBoundaryCollision(simulation, entity, CollisionSide.LeftBorder))
                {
                    entity.Position.X = entity.OldPosition.X = 0;
                    entity.Velocity.X = 0;
                }
            }

            if (entity.Position.X > simulation.World.MapPixelWidth() - entity.Dimensions.X)
            {
                if (physics.OnWorldBoundaryCollision(simulation, entity, CollisionSide.RightBorder))
                {
                    entity.Position.X = entity.OldPosition.X = simulation.World.MapPixelWidth() - entity.Dimensions.X;
                    entity.Velocity.X = 0;
                }
            }

            if (entity.Position.Y < 0)
                physics.OnWorldBoundaryCollision(simulation, entity, CollisionSide.TopBorder);
            
            if (entity.Position.Y > simulation.World.MapPixelHeight())
                physics.OnWorldBoundaryCollision(simulation, entity, CollisionSide.TopBorder);

        }

        #endregion
        
        #region BroadPhaseHorizontal

        private static void BroadPhaseHorizontal(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity)
        {
            
            float tileSize = simulation.World.WorldOptions.WorldScale;
            
            if (entity.Position.X - entity.OldPosition.X < 0)//works now :D
            { //Left step this time

                TileCollision tc = GetTileCollisionTemplate(entity,CollisionSide.RightBorder, tileSize);

                for (int i = tc.TopBlockY; i <= tc.BottomBlockY; i++)
                {
                    if ((tc.Tile = simulation.World.GetBlock(tc.LeftBlockX, i)) == 0) continue;

                    tc.TilePosition.Y = i;
                    
                    if (physics.RightBorderCollision(simulation, entity, tc))
                        return;
                }
            }
            else if(entity.Position.X - entity.OldPosition.X > 0)//Works dont touch!!
            {//Right step this time
                
                TileCollision tc = GetTileCollisionTemplate(entity,CollisionSide.LeftBorder, tileSize);

                for (int i = tc.TopBlockY; i <= tc.BottomBlockY; i++)
                {
                    if ((tc.Tile = simulation.World.GetBlock(tc.RightBlockX, i)) == 0) continue;
                    
                    tc.TilePosition.Y = i;
                    
                    if (physics.LeftBorderCollision(simulation, entity, tc))
                        return;

                }
            }
        }
        
        #endregion
        
        #region BroadPhaseVertical

        private static void BroadPhaseVertical(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity)
        {
            float tileSize = simulation.World.WorldOptions.WorldScale;
            
            if (entity.Position.Y - entity.OldPosition.Y > 0)
            {//movin down
                
                TileCollision tc = GetTileCollisionTemplate(entity,CollisionSide.TopBorder, tileSize);
                
                //in between bounds corners
                for (int i = tc.LeftBlockX; i <= tc.RightBlockX; i++)
                {
                    if ((tc.Tile = simulation.World.GetBlock(i, tc.BottomBlockY)) == 0) 
                        continue;
                    
                    tc.TilePosition.X = i;
                    
                    if (physics.TopBorderCollision(simulation, entity, tc))
                        return;
                }
            }
            else if (entity.Position.Y - entity.OldPosition.Y < 0)
            {//move up
                
                TileCollision tc = GetTileCollisionTemplate(entity,CollisionSide.BottomBorder, tileSize);
                
                //in between bounds corners
                for (int i = tc.LeftBlockX; i <= tc.RightBlockX; i++)
                {
                    if ((tc.Tile = simulation.World.GetBlock(i, tc.TopBlockY)) == 0) 
                        continue;

                    tc.TilePosition.X = i;
                    
                    if (physics.BottomBorderCollision(simulation, entity, tc))
                        return;
                }
            }       
        }
        
        #endregion

        #region GetTileCollisionTemplate
        
        private static TileCollision GetTileCollisionTemplate(SimulationEntity entity, CollisionSide side, float tileSize)
        {
            
            TileCollision tc = new TileCollision
            {
                Side = side,
                TilePosition = new BfbVector(),
                TopBlockY = (int) System.Math.Floor(entity.Top / tileSize),
                BottomBlockY = (int) System.Math.Floor((entity.Bottom - 1) / tileSize),
                LeftBlockX = (int) System.Math.Floor(entity.Left / tileSize),
                RightBlockX = (int) System.Math.Floor((entity.Right - 1) / tileSize)
            };

            switch (side)
            {
                case CollisionSide.BottomBorder:
                    tc.TilePosition.Y = tc.TopBlockY;
                    break;
                case CollisionSide.TopBorder:
                    tc.TilePosition.Y = tc.BottomBlockY;
                    break;
                case CollisionSide.RightBorder:
                    tc.TilePosition.X = tc.LeftBlockX;
                    break;
                case CollisionSide.LeftBorder:
                    tc.TilePosition.X = tc.RightBlockX;
                    break;
            }
    
            return tc;
        }
        
        #endregion

        #region TopCollision
        
        private static bool TopBorderCollision(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            int tileSize = simulation.World.WorldOptions.WorldScale;
            
            // if the object is not moving down
            if (!(entity.Position.Y - entity.OldPosition.Y > 0)) return false;

            Rectangle block = tc.TileRectangle(tileSize);

            if (entity.Bottom <= block.Y || entity.OldBottom > block.Y) return false;
            
            int sectionXPointCount = (int) (entity.Dimensions.X / tileSize) * 2;
            float sectionXOffset = tileSize / 2f;
            
            for (int i = 0; i <= sectionXPointCount; i++)
            {
                if (!IsPointColliding(entity.Left + (i * sectionXOffset), entity.Bottom, block))
                    continue;
                
                if (!physics.OnTileCollision(simulation, entity, tc))
                    return true;

                entity.Velocity.Y = 0;
                entity.Position.Y = entity.OldPosition.Y = block.Y - entity.Height;

                return true;
            }

            return false;
        }
        
        #endregion
        
        #region BottomCollision

        private static bool BottomBorderCollision(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            int tileSize = simulation.World.WorldOptions.WorldScale;
            
            // if the object is not moving up
            if (!(entity.Position.Y - entity.OldPosition.Y < 0)) return false;

            Rectangle block = tc.TileRectangle(tileSize);

            if (entity.Top >= (block.Y + block.Height) || entity.OldTop < (block.Y + block.Height))
                return false;
            
            int sectionXPointCount = (int)( entity.Dimensions.X / tileSize) * 2;
            float sectionXOffset = tileSize/2f;

            for (int i = 0; i <= sectionXPointCount; i++)
            {
                if (!IsPointColliding(entity.Left + (i * sectionXOffset), entity.Top, block))
                    continue;
                
                if (!physics.OnTileCollision(simulation,entity, tc))
                    return true;
                
                entity.Velocity.Y = -0.001f;
                entity.Position.Y = entity.OldPosition.Y = block.Y + block.Height;

                return true;
            }

            return false;
        }
        
        #endregion
        
        #region LeftCollision

        private static bool LeftBorderCollision(this IPhysicsComponent physics, Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            int tileSize = simulation.World.WorldOptions.WorldScale;
            
            //moving entity is not moving right
            if (!(entity.Position.X - entity.OldPosition.X > 0)) return false;

            Rectangle block = tc.TileRectangle(tileSize);

            if (entity.Right <= block.X || entity.OldRight > block.X) return false;
            
            int sectionYPointCount = (int) (entity.Dimensions.Y / tileSize) * 2;
            float sectionYOffset = tileSize / 2f;

            for (int i = 0; i < sectionYPointCount; i++)
            {
                if (!IsPointColliding(entity.Right, entity.Top + (i * sectionYOffset), block))
                    continue;
                
                if (!physics.OnTileCollision(simulation,entity, tc))
                    return true;

                entity.Velocity.X = 0;
                entity.Position.X = entity.OldPosition.X = block.X - entity.Width;

                return true;
            }

            return false;
        }
        
        #endregion
        
        #region RightCollision
 
        private static bool RightBorderCollision(this IPhysicsComponent physics,Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            int tileSize = simulation.World.WorldOptions.WorldScale;
            //if entity is not moving left
            if (!(entity.Position.X - entity.OldPosition.X < 0)) return false;
            
            Rectangle block = tc.TileRectangle(tileSize);

            if (entity.Left >= (block.X + block.Width) || entity.OldLeft < block.X + block.Width) return false;
            
            int sectionYPointCount = (int) (entity.Dimensions.Y / tileSize) * 2;
            float sectionYOffset = tileSize / 2f;
            
            for (int i = 0; i < sectionYPointCount; i++)
            {
                if (!IsPointColliding(entity.Left, entity.Top + (i * sectionYOffset), block))
                    continue;

                if (!physics.OnTileCollision(simulation,entity, tc))
                    return true;
                
                entity.Velocity.X = 0;
                entity.Position.X = entity.OldPosition.X = block.X + tileSize;

                return true;
            }

            return false;
        }

        #endregion

        #region IsPointColliding

        /// <summary>
        /// Detects a collisions with a rectangle and a point
        /// </summary>
        /// <param name="x">The point X position</param>
        /// <param name="y">The point Y position</param>
        /// <param name="block">The rectangle to check the point against</param>
        /// <returns>A boolean indicating if the point is inside the rectangle</returns>
        [UsedImplicitly]
        public static bool IsPointColliding(float x, float y, Rectangle block)
        {
            return x > block.X && x < block.X + block.Width && y > block.Y && y < block.Y + block.Height;
        }
        
        #endregion

        #region IsRectangleColliding

        /// <summary>
        /// Detects if two rectangles colliding
        /// </summary>
        /// <param name="rectangle1">The first rectangle</param>
        /// <param name="rectangle2">The second rectangle</param>
        /// <returns>A boolean indicating if the rectangles are colliding</returns>
        [UsedImplicitly]
        public static bool IsRectangleColliding(Rectangle rectangle1, Rectangle rectangle2)
        {
            return false;
        }
        
        #endregion
    }
    
    
}