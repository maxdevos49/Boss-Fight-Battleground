using BFB.Engine.Content;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameComponents
{
    
    /// <summary>
    /// Automatically updates the walking animation based on the entities velocity
    /// </summary>
    public class WalkingAnimationComponent : IGameComponent
    {
        public void Update(SimulationEntity entity, Simulation simulation)
        {
            
            if (entity.Velocity.X > 2)
            {
                entity.Facing = DirectionFacing.Right;
                entity.AnimationState = AnimationState.MoveRight;
            }
            else if (entity.Velocity.X < -2)
            {
                entity.Facing = DirectionFacing.Left;
                entity.AnimationState = AnimationState.MoveLeft;
            }
            else
            {
                entity.Velocity.X = 0;
                entity.AnimationState = (entity.Facing == DirectionFacing.Left)
                    ? AnimationState.IdleLeft
                    : AnimationState.IdleRight;
            }
        }
    }
}