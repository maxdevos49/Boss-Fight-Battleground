using BFB.Engine.Content;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    
    /// <summary>
    /// Automatically updates the walking animation based on the entities velocity
    /// </summary>
    public class AnimatedWalking : EntityComponent
    {
        public AnimatedWalking(): base(false) { }
        
        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            
            if (entity.Velocity.X > 2)
                entity.AnimationState = AnimationState.MoveRight;
            else if (entity.Velocity.X < -2)
                entity.AnimationState = AnimationState.MoveLeft;
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