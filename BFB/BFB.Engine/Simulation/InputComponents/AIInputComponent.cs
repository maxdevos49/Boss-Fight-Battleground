using System.Numerics;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.InputComponents
{
    // ReSharper disable once InconsistentNaming
    public class AIInputComponent : IInputComponent
    {
        public void Update(SimulationEntity entity, Simulation simulation)
        {
            Vector2 nearest = new Vector2(200, 180);
            var chunkEntities = simulation.World.ChunkIndex[entity.ChunkKey].Entities;
            foreach (var simulationEntity in chunkEntities)
                
            {
                
                //do the distance formula with their position and your positon
                //find the closest one
                //
            }
            //set the desired vector to point towards the closest entity
        }
    }
}