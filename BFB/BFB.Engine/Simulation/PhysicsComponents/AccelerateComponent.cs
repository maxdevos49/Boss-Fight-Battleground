/**
namespace BFB.Engine.Simulation.PhysicsComponents
{
    public class AccelerateComponent : IPhysicsComponent
    {
        
        private readonly float _maxForce;

        private readonly float _maxSpeed;

        public AccelerateComponent()
        {
            _maxForce = 0.7f; 
            _maxSpeed = 10;
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {

            //Enforce max speed
            simulationEntity.DesiredVector.Magnitude = _maxSpeed;
            
            BfbVector steering = BfbVector.Sub(simulationEntity.DesiredVector, simulationEntity.Velocity);

            //enforce max force
            steering.Limit(_maxForce);

            //Apply steering to velocity
            simulationEntity.Velocity.Add(steering);

            //update position
            simulationEntity.Position.Add(simulationEntity.Velocity);

            //update Rotation with degrees
            simulationEntity.Rotation = Convert.ToSingle(System.Math.Atan2(simulationEntity.Velocity.Y, simulationEntity.Velocity.X));
            
        }
    }
}**/