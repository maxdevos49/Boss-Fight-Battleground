using System;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.InputComponents
{
    public class RemoteInputComponent : IInputComponent
    {
        
        #region Properties

        private readonly object _lock;

        private PlayerState _playerState;
        
        #endregion

        
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

                    Tuple<int, int, int, int> chunkInformation =
                        simulation.WorldManager.TranslatePixelPosition((int)_playerState.Mouse.X, (int)_playerState.Mouse.Y);

                    Chunk targetChunk =
                        simulation.WorldManager.ChunkFromChunkLocation(chunkInformation.Item1, chunkInformation.Item2);

                    int xSelection = chunkInformation.Item3;
                    int ySelection = chunkInformation.Item4;

                    if (targetChunk != null)
                    {
                        if (_playerState.RightClick)
                            targetChunk.ApplyBlockUpdate(new TileUpdate
                            {
                                X = (byte) xSelection,
                                Y = (byte) ySelection,
                                Mode = true,
                                TileValue = (ushort) WorldTile.Dirt
                            });
                        else
                            targetChunk.ApplyBlockUpdate(new TileUpdate
                            {
                                X = (byte) xSelection,
                                Y = (byte) ySelection,
                                Mode = true,
                                TileValue = (ushort) WorldTile.Air
                            });
                    }
                }

                //Resets the player movement
                simulationEntity.DesiredVector.X = 0;
                simulationEntity.DesiredVector.Y = 0;
                //Moves player left
                if (_playerState.Left)
                {
                    simulationEntity.DesiredVector.Add(new BfbVector(-1,0));
                }
                //Moves player right
                if (_playerState.Right)
                {
                    simulationEntity.DesiredVector.Add(new BfbVector(1,0));
                }
                //Moves player up
                if (_playerState.Jump && simulationEntity.Grounded)
                {
                    simulationEntity.DesiredVector.Add(new BfbVector(0,-1));
                    simulationEntity.Grounded = false;
                }
            }
        }
    }
}