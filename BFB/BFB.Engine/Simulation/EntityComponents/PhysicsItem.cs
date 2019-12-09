using System;
using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class PhysicsItem : EntityComponent, IPhysics
    {
        public BfbVector Acceleration { get; set; }
        public BfbVector MaxSpeed { get; set; }
        public float Gravity { get; set; }
        public float Friction { get; set; }

        public PhysicsItem() : base(false)
        {
            Acceleration = new BfbVector(5,5);
            MaxSpeed = new BfbVector(20,29);
            Gravity = 4f;
            Friction = 0.8f;
        }

        public override void Init(SimulationEntity entity)
        {
            if ((int)entity.SteeringVector.X != 0 || (int)entity.SteeringVector.Y != 0)
                entity.Velocity = entity.SteeringVector.Clone();
            else
            {
                Random rand = new Random();

                entity.Velocity.X = rand.Next(-3,3) * Acceleration.X;
                entity.Velocity.Y = rand.Next(-3,0) * Acceleration.Y;
            }
            
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            entity.Velocity.Y += Gravity;
            entity.Velocity.X *= Friction;

            if(System.Math.Abs(entity.Velocity.X) > MaxSpeed.X)
                entity.Velocity.X = entity.Velocity.X > 0 ? MaxSpeed.X : -MaxSpeed.X;
            
            if(System.Math.Abs(entity.Velocity.Y) > MaxSpeed.Y)
                entity.Velocity.Y = entity.Velocity.Y > 0 ? MaxSpeed.Y : -MaxSpeed.Y;
            
            entity.Position.Add(entity.Velocity);

            Collision.DetectCollision(simulation, entity);
        }


        public override bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            if (side != CollisionSide.BottomBorder) 
                return true;
            
            simulation.RemoveEntity(entity.EntityId);
            
            return false;
        }
    }
}