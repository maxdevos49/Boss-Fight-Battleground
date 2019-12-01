using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents
{
    /// <summary>
    /// A player physics component that is used for normal walking like physics
    /// </summary>
    public class  WalkingPhysics: EntityComponent,  IPhysics
    {
        public BfbVector Acceleration { get; set; }
        public BfbVector MaxSpeed { get; set; }
        public float Gravity { get; set; }
        public float Friction { get; set; }

        /// <summary>
        /// Constructs a Walking Physics Component
        /// </summary>
        public WalkingPhysics(int maxSpeed = 20) : base(false)
        {
            Acceleration = new BfbVector(5,25);
            MaxSpeed = new BfbVector(maxSpeed,29);
            Gravity = 4f;
            Friction = 0.8f;
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
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
            Collision.DetectCollision(simulation, entity);
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            entity.Grounded = entity.Velocity.Y == 0.0f;
        }
    }

}