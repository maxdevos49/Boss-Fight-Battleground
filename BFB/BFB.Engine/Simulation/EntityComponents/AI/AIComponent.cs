using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.TileMap;
using static System.Int32;

namespace BFB.Engine.Simulation.EntityComponents.AI
{
    public class AIInputComponent : EntityComponent
    {
        private SimulationEntity _closestEntity;

        public AIInputComponent() : base(false) { }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {

            int nearest = MaxValue;

            List<Chunk> chunkList = new List<Chunk>();
            Chunk chunk = simulation.World.ChunkIndex[entity.ChunkKey];
            chunkList.Add(chunk);

            //check right upper point chunk
            Chunk chunkTest = simulation.World.ChunkFromPixelLocation(entity.Right, entity.Top);
            if (chunk.ChunkY != chunkTest?.ChunkX || chunk.ChunkY != chunkTest.ChunkY)
                chunkList.Add(chunkTest);

            //check lower right chunk
            chunkTest = simulation.World.ChunkFromPixelLocation(entity.Right, entity.Bottom);
            if (chunk.ChunkY != chunkTest?.ChunkX || chunk.ChunkY != chunkTest.ChunkY)
                chunkList.Add(chunkTest);

            //check bottom left chunk
            chunkTest = simulation.World.ChunkFromPixelLocation(entity.Left, entity.Right);
            if (chunk.ChunkY != chunkTest?.ChunkX || chunk.ChunkY != chunkTest.ChunkY)
                chunkList.Add(chunkTest);

            foreach (Chunk chunk1 in chunkList)
            {
                if (chunk1 == null)
                    continue;

                foreach (var item in chunk1.Entities.ToList().Where(x => x.Value.EntityId != entity.EntityId && x.Value.TextureKey == "Player"))
                {
                    var distance = System.Math.Sqrt(System.Math.Pow(item.Value.Position.X - entity.Position.X,2) + System.Math.Pow(item.Value.Position.Y - entity.Position.Y, 2));
                    if (distance < nearest)
                    {
                        nearest = (int)distance;
                        _closestEntity = item.Value;
                    }
                }
                
            }

            if(_closestEntity != null)
            {
                //Resets the entity movement
                entity.SteeringVector.X = 0;
                entity.SteeringVector.Y = 0;
                //Moves entity left
                if (_closestEntity.Position.X < entity.Position.X)
                {
                    entity.SteeringVector.Add(new BfbVector(-1,0));
                }
                //Moves entity right
                if (_closestEntity.Position.X > entity.Position.X)
                {
                    entity.SteeringVector.Add(new BfbVector(1,0));
                }

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_closestEntity.Velocity.X == 0 && entity.Grounded)
                {
                    entity.SteeringVector.Add(new BfbVector(0,-1));
                }
            }
        }
    }
}