using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    public class TilePhysicsComponent : IPhysicsComponent
    {
        public BfbVector Acceleration { get; set; }
        public BfbVector MaxSpeed { get; set; }
        public float Gravity { get; set; }
        public float Friction { get; set; }
        public string CollideFilter { get; set; }
        public List<string> CollideWithFilters { get; set; }

        private bool _initialVelocity;

        public TilePhysicsComponent()
        {
            CollideFilter = "item";
            CollideWithFilters = new List<string> {"monster","human","tile"};
            Acceleration = new BfbVector(5,5);
            MaxSpeed = new BfbVector(20,29);
            Gravity = 4f;
            Friction = 0.8f;
            _initialVelocity = false;
        }

        public void Update(SimulationEntity entity, Simulation simulation)
        {
            //Give a slight movement change on initial creation
            if (!_initialVelocity)
            {
                Random rand = new Random();

                entity.Velocity.X = rand.Next(-3,3) * Acceleration.X;
                entity.Velocity.Y = rand.Next(-3,0) * Acceleration.Y;
                
                _initialVelocity = true;
            }
            
            entity.Velocity.Y += Gravity;
            entity.Velocity.X *= Friction;

            if(System.Math.Abs(entity.Velocity.X) > MaxSpeed.X)
                entity.Velocity.X = entity.Velocity.X > 0 ? MaxSpeed.X : -MaxSpeed.X;
            
            if(System.Math.Abs(entity.Velocity.Y) > MaxSpeed.Y)
                entity.Velocity.Y = entity.Velocity.Y > 0 ? MaxSpeed.Y : -MaxSpeed.Y;
            
            entity.Position.Add(entity.Velocity);

            this.DetectCollision(simulation, entity);
        }

        public bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            return true;
        }

        public bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tileCollision)
        {
            return true;
        }

        public bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            if (side != CollisionSide.BottomBorder) 
                return true;
            
            simulation.RemoveEntity(entity.EntityId);
            
            return false;
        }
    }
}