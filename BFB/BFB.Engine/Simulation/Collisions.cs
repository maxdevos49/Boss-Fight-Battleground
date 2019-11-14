using System.Drawing;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation
{
    public static class Collision
    {
        
        #region DetectCollision

        /// <summary>
        /// Does Broadphase collision detection and if collisions are detected then it dispatches Narrowphase collision detection
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="simulation"></param>
        public static void DetectCollision(SimulationEntity entity, Simulation simulation)
        {
            
            //World Boundaries
            if (entity.Position.X < 0)
            {
                entity.Position.X = entity.OldPosition.X = 0;
                entity.Velocity.X = 0;
            }

            if (entity.Position.X > simulation.World.MapPixelWidth() - entity.Dimensions.X)
            {
                entity.Position.X = entity.OldPosition.X = simulation.World.MapPixelWidth() - entity.Dimensions.X;
                entity.Velocity.X = 0;
            }


            //Left and right collisions
            BroadPhaseHorizontal(entity,simulation);
            
            //Detect up and down
            BroadPhaseVertical(entity,simulation);
        }

        #endregion
        
        #region BroadPhaseHorizontal

        private static void BroadPhaseHorizontal(SimulationEntity entity, Simulation simulation)
        {
            
            float tileSize = simulation.World.WorldOptions.WorldScale;
            
            if (entity.Position.X - entity.OldPosition.X < 0)//works now :D
            { //Left step this time

                //Get search Bounds
                int topBlockY = (int) System.Math.Floor(entity.Top / tileSize);
                int leftBlockX = (int) System.Math.Floor(entity.Left / tileSize);
                int bottomBlockY = (int) System.Math.Floor((entity.Bottom - 1) / tileSize);
                
                for (int i = topBlockY; i <= bottomBlockY; i++)
                    if (simulation.World.GetBlock(leftBlockX, i) != 0)
                            if (RightBorderCollision(entity, i, leftBlockX, (int) tileSize))
                                return;

            }
            else if(entity.Position.X - entity.OldPosition.X > 0)//Works dont touch!!
            {//Right step this time
                
                //Get search Bounds
                int topBlockY = (int) System.Math.Floor(entity.Top / tileSize);
                int rightBlockX = (int) System.Math.Floor((entity.Right - 1) / tileSize);
                int bottomBlockY = (int) System.Math.Floor((entity.Bottom - 1) / tileSize);
            
                for (int i = topBlockY; i <= bottomBlockY; i++)
                    if (simulation.World.GetBlock(rightBlockX, i) != 0)
                        if (LeftBorderCollision(entity, i, rightBlockX,(int) tileSize))
                            return;

            }
        }
        
        #endregion
        
        #region BroadPhaseVertical

        private static void BroadPhaseVertical(SimulationEntity entity, Simulation simulation)
        {
            float tileSize = simulation.World.WorldOptions.WorldScale;
            
            if (entity.Position.Y - entity.OldPosition.Y > 0)
            {//movin down
                
                //Get search Bounds
                int leftBlockX = (int) System.Math.Floor(entity.Left / tileSize);
                int rightBlockX = (int) System.Math.Floor((entity.Right - 1) / tileSize);
                int bottomBlockY = (int) System.Math.Floor((entity.Bottom - 1) / tileSize);
                
                //in between bounds corners
                for (int i = leftBlockX; i <= rightBlockX; i++)
                    if ((int) simulation.World.GetBlock(i, bottomBlockY) != 0)
                        if (TopBorderCollision(entity, bottomBlockY, i, (int) tileSize))
                            return;
            }
            else if (entity.Position.Y - entity.OldPosition.Y < 0)
            {//move up
                
                //Get search Bounds
                int leftBlockX = (int) System.Math.Floor(entity.Left / tileSize);
                int rightBlockX = (int) System.Math.Floor((entity.Right - 1) / tileSize);
                int topBlockY = (int) System.Math.Floor(entity.Top / tileSize);
                
                //in between bounds corners
                for (int i = leftBlockX; i <= rightBlockX; i++)
                    if ((int) simulation.World.GetBlock(i, topBlockY) != 0)
                        if (BottomBorderCollision(entity, topBlockY,i, (int) tileSize))
                            return;
            }       
        }
        
        #endregion

        #region TopCollision
        
        private static bool TopBorderCollision(SimulationEntity entity, int row, int column,  int tileSize)
        {
            // if the object is not moving down
            if (!(entity.Position.Y - entity.OldPosition.Y > 0)) return false;
            
            Rectangle block = new Rectangle
            {
                X = column * tileSize,
                Y = row * tileSize,
                Width = tileSize,
                Height = tileSize
            };

            if (entity.Bottom <= block.Y || entity.OldBottom > block.Y) return false;
            
            int sectionXPointCount = (int) (entity.Dimensions.X / tileSize) * 2;
            float sectionXOffset = tileSize / 2f;
            
            for (int i = 0; i <= sectionXPointCount; i++)
            {
                
                if (!IsPointColliding(entity.Left + (i * sectionXOffset), entity.Bottom, block))
                    continue;

                entity.Velocity.Y = 0;
                entity.Position.Y = entity.OldPosition.Y = block.Y - entity.Height;

                return true;
            }

            return false;
        }
        
        #endregion
        
        #region BottomCollision

        private static bool BottomBorderCollision(SimulationEntity entity, int row, int column, int tileSize)
        {
            // if the object is not moving up
            if (!(entity.Position.Y - entity.OldPosition.Y < 0)) return false;
            
            Rectangle block = new Rectangle
            {
                X = column * tileSize,
                Y = row * tileSize,
                Width = tileSize,
                Height = tileSize
            };

            if (entity.Top >= (block.Y + block.Height) || entity.OldTop < (block.Y + block.Height)) return false;
            
            int sectionXPointCount = (int)( entity.Dimensions.X / tileSize) * 2;
            float sectionXOffset = tileSize/2f;

            for (int i = 0; i <= sectionXPointCount; i++)
            {
                if (!IsPointColliding(entity.Left + (i * sectionXOffset), entity.Top, block)) continue;
                
                entity.Velocity.Y = -0.001f;
                entity.Position.Y = entity.OldPosition.Y = block.Y + block.Height;

                return true;
            }

            return false;
        }
        
        #endregion
        
        #region LeftCollision

        private static bool LeftBorderCollision(SimulationEntity entity, int row, int column, int tileSize)
        {
            //moving entity is not moving right
            if (!(entity.Position.X - entity.OldPosition.X > 0)) return false;
           
            Rectangle block = new Rectangle
            {
                X = column * tileSize,
                Y = row * tileSize,
                Width = tileSize,
                Height = tileSize
            };

            if (entity.Right <= block.X || entity.OldRight > block.X) return false;
            
            int sectionYPointCount = (int) (entity.Dimensions.Y / tileSize) * 2;
            float sectionYOffset = tileSize / 2f;

            for (int i = 0; i < sectionYPointCount; i++)
            {
                if (!IsPointColliding(entity.Right, entity.Top + (i * sectionYOffset), block))
                    continue;

                entity.Velocity.X = 0;
                entity.Position.X = entity.OldPosition.X = block.X - entity.Width;

                return true;
            }

            return false;
        }
        
        #endregion
        
        #region RightCollision

 
        private static bool RightBorderCollision(SimulationEntity entity, int row, int column, int tileSize)
        {
            //if entity is not moving left
            if (!(entity.Position.X - entity.OldPosition.X < 0)) return false;
            
            Rectangle block = new Rectangle
            {
                X = column * tileSize,
                Y = row * tileSize,
                Width = tileSize,
                Height = tileSize
            };

            if (entity.Left >= (block.X + block.Width) || entity.OldLeft < block.X + block.Width) return false;
            
            int sectionYPointCount = (int) (entity.Dimensions.Y / tileSize) * 2;
            float sectionYOffset = tileSize / 2f;
            
            for (int i = 0; i < sectionYPointCount; i++)
            {
                if (!IsPointColliding(entity.Left, entity.Top + (i * sectionYOffset), block))
                    continue;
                
                entity.Velocity.X = 0;
                entity.Position.X = entity.OldPosition.X = block.X + tileSize;

                return true;
            }

            return false;
        }

        #endregion
        
        private static bool IsPointColliding(float x, float y, Rectangle block)
        {
            return x > block.X && x < block.X + block.Width && y > block.Y && y < block.Y + block.Height;
        }
    }
    
    
}