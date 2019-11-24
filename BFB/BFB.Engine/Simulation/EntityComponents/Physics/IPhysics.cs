using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents.Physics
{
    /// <summary>
    /// The interface to outline how to implement a Physics component
    /// </summary>
    public interface IPhysics
    { 
        
        /// <summary>
        /// Represents the acceleration in the two axis independently
        /// </summary>
        BfbVector Acceleration { get; set; }
        
        /// <summary>
        /// Represents the maximum speed the entity can move in each axis
        /// </summary>
        BfbVector MaxSpeed { get; set; }
        
        /// <summary>
        /// Indicates the strength of gravity of the tile
        /// </summary>
        float Gravity { get; set; }
        
        /// <summary>
        /// Indicates the amount of friction the tile has when moving horizontally
        /// </summary>
        float Friction { get; set; }

    }
}