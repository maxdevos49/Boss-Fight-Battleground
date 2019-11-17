using System;
using System.Collections.Generic;
using System.Security.Claims;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.GameComponents;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.InputComponents
{
    /// <summary>
    /// A Input component that is used for remotely controlling a entity from a socket connection
    /// </summary>
    public class RemoteInputComponent : IInputComponent
    {
        
        #region Properties

        private readonly object _lock;

        private PlayerState _playerState;
        
        #endregion

        /// <summary>
        /// Constructs a RemoteInputComponent using a ClientSocket
        /// </summary>
        /// <param name="socket">The controlling players ClientSocket</param>
        public RemoteInputComponent(ClientSocket socket)
        {
            _lock = new object();
            _playerState = new PlayerState();
            
            socket.On("/player/input", (m) =>
            {
                InputMessage mm = (InputMessage) m;
                lock (_lock)
                {
                    _playerState = mm.PlayerInputState;//Update new position
                }
            });
            
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            lock (_lock)
            {
                //Check block placement
                if (_playerState.RightClick || _playerState.LeftClick)
                {

                    int mouseX = (int)_playerState.Mouse.X;
                    int mouseY = (int)_playerState.Mouse.Y;

                    Tuple<int, int, int, int> chunkInformation =
                        simulation.World.TranslatePixelPosition(mouseX, mouseY);

                    if (chunkInformation != null)
                    {

                        Chunk targetChunk =
                            simulation.World.ChunkFromChunkLocation(chunkInformation.Item1, chunkInformation.Item2);

                        int xSelection = chunkInformation.Item3;
                        int ySelection = chunkInformation.Item4;

                        if (targetChunk != null)
                        {
                            if (_playerState.RightClick)
                            {

                                if (targetChunk.Block[xSelection, ySelection] == 0)
                                {
                                    targetChunk.ApplyBlockUpdate(new TileUpdate
                                    {
                                        X = (byte) xSelection,
                                        Y = (byte) ySelection,
                                        Mode = true,
                                        TileValue = (ushort) WorldTile.Dirt
                                    });
                                }
                            }
                            else
                            {
                                if (targetChunk.Block[xSelection, ySelection] != 0)
                                {
                                    targetChunk.ApplyBlockUpdate(new TileUpdate
                                    {
                                        X = (byte) xSelection,
                                        Y = (byte) ySelection,
                                        Mode = true,
                                        TileValue = (ushort) WorldTile.Air
                                    });

                                    int blockX =
                                        ((simulation.World.WorldOptions.ChunkSize * chunkInformation.Item1) +
                                         xSelection) *
                                        simulation.World.WorldOptions.WorldScale;
                                    int blockY =
                                        ((simulation.World.WorldOptions.ChunkSize * chunkInformation.Item2) +
                                         ySelection) *
                                        simulation.World.WorldOptions.WorldScale;

                                    simulation.AddEntity(new SimulationEntity(
                                        Guid.NewGuid().ToString(),
                                        new EntityOptions
                                        {
                                            AnimatedTextureKey = "Player",
                                            Position = new BfbVector(blockX, blockY),
                                            Dimensions = new BfbVector(1 * simulation.World.WorldOptions.WorldScale,
                                                1 * simulation.World.WorldOptions.WorldScale),
                                            Rotation = 0,
                                            Origin = new BfbVector(0, 0),
                                            EntityType = EntityType.Item
                                        }, new ComponentOptions
                                        {
                                            Physics = new TilePhysicsComponent(),
                                            Input = null,
                                            GameComponents = new List<IGameComponent>
                                            {
                                                new LifetimeComponent(1000)
                                            }
                                        }));
                                }
                            }
                        }
                    }
                }

                //Resets the player movement
                simulationEntity.SteeringVector.X = 0;
                simulationEntity.SteeringVector.Y = 0;
                
                //Moves player left
                if (_playerState.Left)
                    simulationEntity.SteeringVector.Add(new BfbVector(-1,0));
                
                //Moves player right
                if (_playerState.Right)
                    simulationEntity.SteeringVector.Add(new BfbVector(1,0));
                
                //Moves player up
                if (_playerState.Jump && simulationEntity.Grounded)
                    simulationEntity.SteeringVector.Add(new BfbVector(0, -1));
            }
        }
    }
}