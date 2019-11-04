using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    public class  PlayerPhysicsComponent: IPhysicsComponent
    {
        private readonly BfbVector _acceleration;
        private readonly BfbVector _maxSpeed;
        private readonly BfbVector _gravity;
        private readonly float _friction;
        private AnimationState _previousAnimationState;
        
        
        public PlayerPhysicsComponent()
        {
            _acceleration = new BfbVector(3,25);
            _maxSpeed = new BfbVector(20,40);
            _gravity = new BfbVector(0, 4f);
            _friction = 0.2f;

        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            
            //Gives us the speed to move left and right
            simulationEntity.DesiredVector.X *= _acceleration.X;
            simulationEntity.DesiredVector.Y *= _acceleration.Y;
            
            if (!simulationEntity.Grounded)
                simulationEntity.DesiredVector.Add(_gravity);
            
            //Creates the new velocity
            simulationEntity.Velocity.Add(simulationEntity.DesiredVector);

            //Caps your speed
            if(System.Math.Abs(simulationEntity.Velocity.X) > _maxSpeed.X)
                simulationEntity.Velocity.X = simulationEntity.Velocity.X > 0 ? _maxSpeed.X : -_maxSpeed.X;
            
            if(System.Math.Abs(simulationEntity.Velocity.Y) > _maxSpeed.Y)
                simulationEntity.Velocity.Y = simulationEntity.Velocity.Y > 0 ? _maxSpeed.Y : -_maxSpeed.Y;
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (simulationEntity.Grounded && simulationEntity.DesiredVector.X == 0)
            {
                //This will affects changing directions back and forth -- so its like smash bros switching directions
                simulationEntity.Velocity.Mult(_friction);
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (System.Math.Abs(simulationEntity.Velocity.X) < 1 && simulationEntity.Velocity.X != 0)
            {
                simulationEntity.Velocity.X = 0;
                simulationEntity.AnimationState = _previousAnimationState == AnimationState.MoveLeft ? AnimationState.IdleLeft : AnimationState.IdleRight;
            }

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

            if(simulationEntity.Position.Y > 200)
            {
                simulationEntity.Position.Y = 200;
                simulationEntity.Velocity.Y = 0;
                simulationEntity.Grounded = true;
            }

            //Animation states
            if (simulationEntity.Velocity.X > 1)
                simulationEntity.AnimationState = AnimationState.MoveRight;
            else if (simulationEntity.Velocity.X < -1)
                simulationEntity.AnimationState = AnimationState.MoveLeft;

            _previousAnimationState = simulationEntity.AnimationState;

        }
    }
}