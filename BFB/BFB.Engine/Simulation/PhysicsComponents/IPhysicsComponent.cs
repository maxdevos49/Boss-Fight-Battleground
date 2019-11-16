using System.Collections.Generic;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    /// <summary>
    /// The interface to outline how to implement a Physics component
    /// </summary>
    public interface IPhysicsComponent
    {
     
        /// <summary>
        /// Indicates the collision group the entity is in
        /// </summary>
        string CollideFilter { get; set; }
        
        /// <summary>
        /// Indicates other groups the entity should collide with
        /// </summary>
        List<string> CollideWithFilters { get; set; }
        
        /// <summary>
        /// The update method that is called each frame of the simulation
        /// </summary>
        /// <param name="simulationEntity">The entity that the physics will apply too</param>
        /// <param name="simulation">An instance of the current simulation</param>
        void Update(SimulationEntity simulationEntity, Simulation simulation);

        /// <summary>
        /// Called whenever two entities collide
        /// </summary>
        /// <param name="simulation">The active simulation</param>
        /// <param name="entityCollision">Data related to the two entities colliding</param>
        /// <returns>whether any default actions should be performed</returns>
        bool OnEntityCollision(Simulation simulation, EntityCollision entityCollision);

        /// <summary>
        /// Called whenever a entity collides with a tile
        /// </summary>
        /// <param name="simulation">The active simulation</param>
        /// <param name="entity">The entity involved in the collision</param>
        /// <param name="tileCollision">Information related the collision</param>
        /// <returns>A Boolean representing if any default actions should be performed</returns>
        bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tileCollision);

        /// <summary>
        /// Called whenever a entity leaves the bounds of the world
        /// </summary>
        /// <param name="simulation">The active simulation</param>
        /// <param name="entity">The entity leaving the world boundry</param>
        /// <param name="side">The side of the world being left</param>
        /// <returns></returns>
        bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side);

    }
}