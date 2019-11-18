using System;
using System.Collections.Generic;
using System.Security.Claims;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;
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
        
        #region Constructor

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
        
        #endregion

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            lock (_lock)
            {
                #region Tom SpawnMonster
                
                //Add an AI monster//TODO
//                if (_playerState.RightClick)
//                {
//
//                    //Add to simulation
//                    simulation.AddEntity(new SimulationEntity(
//                        Guid.NewGuid().ToString(),
//                        new EntityOptions
//                        {
//                            AnimatedTextureKey = "Skeleton",
//                            Position = new BfbVector(_playerState.Mouse.X, _playerState.Mouse.Y),
//                            Dimensions = new BfbVector(2 * simulation.World.WorldOptions.WorldScale, 3 * simulation.World.WorldOptions.WorldScale),
//                            Rotation = 0,
//                            Origin = new BfbVector(0, 0),
//                        }, new ComponentOptions
//                        {
//                            Physics = new WalkingPhysicsComponent(),
//                            Input = new AIInputComponent()
//                        }));
//                }

                #endregion

                //Check block placement
                if (_playerState.RightClick || _playerState.LeftClick)
                {
                    
                    #region Combat
                    
                    //Combat
                    if (_playerState.LeftClick)
                    {
                        List<SimulationEntity> targets = new List<SimulationEntity>();
                        //check each pixel 100 pixels in front of the player
                        for (int i = 0; i < 100; i++)
                        {
                            int xPos = (int) simulationEntity.Position.X + i;
                            if (simulationEntity.Facing == DirectionFacing.Left)
                                xPos = (int) simulationEntity.Position.X - i;

                            //get possible entity at location
                            SimulationEntity target = simulation.GetEntityAtPosition(xPos, (int) simulationEntity.Position.Y);
                            
                            //determine if to add the entities
                            if (target != null && target != simulationEntity && !targets.Contains(target))
                                targets.Add(target);
                        }

                        //Apply actual damage
                        Helpers.CombatService.FightPeople(simulationEntity, targets, simulation);
                        
                    }
                    
                    #endregion

                    #region Break or place Blocks
                    
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
                            
                            //creat block
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
                            {//Break block
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

                                    #region Create broken block Entity
                                    
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
                                            GameComponents = new List<ISimulationComponent>
                                            {
                                                new LifetimeComponent(1000)
                                            }
                                        }));
                                    
                                    #endregion
                                }
                            }
                        }
                    }
                    
                    #endregion
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