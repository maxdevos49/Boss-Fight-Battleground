using System;
using BFB.Engine.Entity;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation
{
    public static class Collision
    {
        
        #region DetectCollision

        public static bool DetectCollision(SimulationEntity entity, Simulation simulation)
        {
            float tileSize = simulation.World.WorldOptions.WorldScale;
            

            //Vertical Test
            if (entity.Position.Y - entity.OldPosition.Y > 0)
            {//Down movement
                
                int bottomRow     = (int)System.Math.Floor(entity.Bottom / tileSize);
                int leftColumn    = (int)System.Math.Floor(entity.Left / tileSize);

                // Check the bottom left point
                if ((int) simulation.World.GetBlock(leftColumn, bottomRow) != 0)
                    TopCollision(entity, bottomRow, (int) tileSize); 

                int rightColumn = (int)System.Math.Floor(entity.Right / tileSize) - 1;

                // Check the bottom right point
                if ((int) simulation.World.GetBlock(rightColumn, bottomRow) != 0)
                    TopCollision(entity, bottomRow, (int) tileSize);

            }
            else if(entity.Position.Y - entity.OldPosition.Y < 0)
            {//Up Movement
                
                int leftColumn    = (int)System.Math.Floor(entity.Left / tileSize);
                int topRow        = (int)System.Math.Floor(entity.Top / tileSize);

                // Check the top left point
                if ((int)simulation.World.GetBlock(leftColumn, topRow) != 0)
                    BottomCollision(entity,topRow,(int)tileSize);

                int rightColumn = (int)System.Math.Floor(entity.Right / tileSize) - 1;
                
                // Check the top right point
                if ((int)simulation.World.GetBlock(rightColumn, topRow) != 0) 
                    BottomCollision(entity,topRow,(int)tileSize);
                
            }
            
            //Horizontal Test
            if (entity.OldPosition.X - entity.Position.X < 0)
            {//Right movement
                return false;
            }
            else
            {//Left Movement
                return false;
            }
        }
        
        #endregion
        
        #region TopCollision
        
        /// <summary>
        /// Detects collisions on the top of a row elevation
        /// </summary>
        /// <param name="entity">The entity checking for collisions</param>
        /// <param name="row">The map row value</param>
        /// <param name="tileSize">The map scale in pixels</param>
        /// <returns>Boolean whether the entity collided or not</returns>
        [UsedImplicitly]
        public static void TopCollision(SimulationEntity entity, int row, int tileSize)
        {

            // if the object is not moving down
            if (!(entity.Position.Y - entity.OldPosition.Y > 0)) return;
            
            // the top side of the specified tile row
            int top = row * tileSize;

            // if the object has passed through the tile boundary since the last game cycle
            if (entity.Bottom <= top || entity.OldBottom > top) return;
                
            entity.Velocity.Y = 0;
            entity.Position.Y = entity.OldPosition.Y = top - entity.Height - 0.01f;
        }
        
        #endregion
        
        #region BottomCollision

        [UsedImplicitly]
        public static void BottomCollision(SimulationEntity entity, int row, int tileSize)
        {
            // if the object is not moving up
            if (!(entity.Position.Y - entity.OldPosition.Y < 0)) return;
            
            int bottom = (row + 1) * tileSize;

            if (entity.Top >= bottom || entity.OldTop < bottom) return;
            
            entity.Velocity.Y = 0;
            entity.Position.Y = entity.OldPosition.Y = bottom;
        }
        
        #endregion
    }
}