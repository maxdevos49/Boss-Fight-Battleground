using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.Collisions;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    public class  ZombiePhysicsComponent: IPhysicsComponent
    {
        public BfbVector Acceleration { get; set; }
        public BfbVector MaxSpeed { get; set; }
        public float Gravity { get; set; }
        public float Friction { get; set; }
        
        public string CollideFilter { get; set; }
        public List<string> CollideWithFilters { get; set; }
        
        
        public ZombiePhysicsComponent()
        {
            CollideFilter = "entity";
            CollideWithFilters = new List<string> {"tile"};
            Acceleration = new BfbVector(5,25);
            MaxSpeed = new BfbVector(20,29);
            Gravity = 4f;
            Friction = 0.8f;
        }

        public void Update(SimulationEntity entity, Simulation simulation)
        {
            
            entity.SteeringVector.X *= Acceleration.X;
            entity.SteeringVector.Y *= Acceleration.Y;
            
            entity.Velocity.Add(entity.SteeringVector);
            
            entity.Velocity.Y += Gravity;
            entity.Velocity.X *= Friction;

            //Caps your speed in specific axis
            if(System.Math.Abs(entity.Velocity.X) > MaxSpeed.X)
                entity.Velocity.X = entity.Velocity.X > 0 ? MaxSpeed.X : -MaxSpeed.X;
            
            if(System.Math.Abs(entity.Velocity.Y) > MaxSpeed.Y)
                entity.Velocity.Y = entity.Velocity.Y > 0 ? MaxSpeed.Y : -MaxSpeed.Y;
            
            entity.Position.Add(entity.Velocity);

            //Collision Test
            this.DetectCollision(simulation, entity);
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            entity.Grounded = entity.Velocity.Y == 0.0f;

        }

        public bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            simulation.RemoveEntity(secondaryEntity.EntityId);
            
            return true;
        }

        public bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tc)
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

        public bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            //If player is below map by at least 50 pixels
            if (side == CollisionSide.BottomBorder && entity.Position.Y > simulation.World.MapPixelHeight() + 50)
            {
                //In future we kill the player and initiate a respawn sequence probably
                entity.Position.Y = 0;
                entity.Position.X = 100;
            }
            
            return true;
        }
    }
}