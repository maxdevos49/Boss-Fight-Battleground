using System;
using System.Collections.Generic;
using System.Drawing;
using BFB.Engine.Math;
using BFB.Engine.Simulation.InputComponents;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Entity
{
    public class SimulationEntity : Entity
    {

        #region Properties

        private int _lastTick;
        
        public bool IsPlayer { get; set; }
        
        public BfbVector DesiredVector { get; }

        public BfbVector OldPosition { get; private set; }

        public List<string> VisibleChunks { get; }

        public Dictionary<string, int> ChunkVersions { get; }

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y);
        
        public int OldBottom => (int)(OldPosition.Y + Height);
        public int OldLeft => (int)(OldPosition.X);
        public int OldRight => (int)(OldPosition.X + Width);
        public int OldTop => (int)(OldPosition.Y);

        #endregion

        #region Components

        private readonly IInputComponent _input;
        private readonly IPhysicsComponent _physics;

        #endregion

        #region Constructor

        public SimulationEntity(string entityId, EntityOptions options, ComponentOptions components) : base(entityId,
            options)
        {
            //Components
            _input = components.Input;
            _physics = components.Physics;

            DesiredVector = new BfbVector();
            OldPosition = new BfbVector();
            VisibleChunks = new List<string>();
            ChunkVersions = new Dictionary<string, int>();

            _lastTick = -1;
        }

        #endregion

        #region Update

        public void Tick(Simulation.Simulation simulation)
        {
            //Only tick entity once per frame
            if (simulation.Tick == _lastTick)
                return;

            //Record last tick
            _lastTick = simulation.Tick;

            //Record current position
            OldPosition = new BfbVector(Position.X, Position.Y);

            //Component Processing
            _input?.Update(this, simulation);
            _physics?.Update(this, simulation);

            //Place entity in correct chunk if in new position
            string chunkKey =
                simulation.World.ChunkFromPixelLocation((int) Position.X, (int) Position.Y)
                    ?.ChunkKey; //If this is null then we are outside of map... Bad

            if (chunkKey != ChunkKey && chunkKey != null)
            {
                simulation.World.MoveEntity(EntityId, simulation.World.ChunkIndex[ChunkKey],
                    simulation.World.ChunkIndex[chunkKey]);
                ChunkKey = chunkKey;
            }

            if (!IsPlayer || ChunkKey == null) return;

            //Clear visible chunks so we dont have to figure out which chunks are no longer being seen
            VisibleChunks.Clear();
            Chunk rootChunk = simulation.World.ChunkIndex[ChunkKey];

            //find the chunks that the player is currently simulating
            for (int y = rootChunk.ChunkY - simulation.SimulationDistance;
                y < rootChunk.ChunkY + simulation.SimulationDistance;
                y++)
            {
                for (int x = rootChunk.ChunkX - simulation.SimulationDistance;
                    x < rootChunk.ChunkX + simulation.SimulationDistance;
                    x++)
                {
                    //Get a chunk if it exist at the location
                    Chunk visibleChunk;
                    if ((visibleChunk = simulation.World.ChunkFromChunkLocation(x, y)) == null)
                        continue;

                    //Add chunk to visible chunks
                    VisibleChunks.Add(visibleChunk.ChunkKey);

                    //update chunk history if new with a negative number so a full chunk update will be forced
                    if (!ChunkVersions.ContainsKey(visibleChunk.ChunkKey))
                        ChunkVersions[visibleChunk.ChunkKey] = -1;
                }
            }
        }

        #endregion

    }

}