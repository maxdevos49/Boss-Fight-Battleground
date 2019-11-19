using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.SimulationComponents
{
    /// <summary>
    /// A Input component that is used for remotely controlling a entity from a socket connection
    /// </summary>
    public class RemoteInputComponent : SimulationComponent
    {

        private ControlState _controlState;

        /// <summary>
        /// Constructs a RemoteInputComponent
        /// </summary>
        public RemoteInputComponent() : base(true)
        {
            _controlState = new ControlState();
        }
        

        public override void Init(SimulationEntity entity)
        {
            entity.Socket?.On("/player/input", (m) =>
            {
                InputMessage mm = (InputMessage) m;
                _controlState = mm.ControlInputState;
            });
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            
            //Update entities player state
            entity.ControlState = _controlState.Clone();
            
            if (entity.ControlState == null)
                return;
            
            //Resets the player movement
            entity.SteeringVector.X = 0;
            entity.SteeringVector.Y = 0;
            
            //Moves player left
            if (entity.ControlState.Left)
                entity.SteeringVector.Add(new BfbVector(-1,0));
                
            //Moves player right
            if (entity.ControlState.Right)
                entity.SteeringVector.Add(new BfbVector(1,0));
                
            //Moves player up
            if (entity.ControlState.Jump && entity.Grounded)
                entity.SteeringVector.Add(new BfbVector(0, -1));
            
            
            //Move out following
            #region Tom SpawnMonster
            
//            //Add an AI monster//TODO
//                if (_playerState.RightClick)
//                {
//
//                    //Add to simulation
//                    simulation.AddEntity(new SimulationEntity(
//                        Guid.NewGuid().ToString(),
//                        new EntityOptions
//                        {
//                            TextureKey = "Skeleton",
//                            Position = new BfbVector(_playerState.Mouse.X, _playerState.Mouse.Y),
//                            Dimensions = new BfbVector(2 * simulation.World.WorldOptions.WorldScale, 3 * simulation.World.WorldOptions.WorldScale),
//                            Rotation = 0,
//                            Origin = new BfbVector(0, 0),
//                            EntityType = EntityType.Mob
//                        }, new ComponentOptions
//                        {
//                            Physics = new WalkingPhysicsComponent(),
//                            Input = new AIInputComponent(),
//                            GameComponents = new List<ISimulationComponent>
//                            {
//                                new WalkingAnimationComponent()
//                            }
//                        }));
//                }

            #endregion

            #region BlockEdit
            //Check block placement
//                if (_playerState.RightClick || _playerState.LeftClick)
//                {
//                   
//
//                    #region Break or place Blocks
//                    
//                    int mouseX = (int)_playerState.Mouse.X;
//                    int mouseY = (int)_playerState.Mouse.Y;
//
//                    Tuple<int, int, int, int> chunkInformation =
//                        simulation.World.TranslatePixelPosition(mouseX, mouseY);
//
//                    if (chunkInformation != null)
//                    {
//
//                        Chunk targetChunk =
//                            simulation.World.ChunkFromChunkLocation(chunkInformation.Item1, chunkInformation.Item2);
//
//                        int xSelection = chunkInformation.Item3;
//                        int ySelection = chunkInformation.Item4;
//
//                        if (targetChunk != null)
//                        {
//                            
//                            //creat block
//                            if (_playerState.RightClick)
//                            {
//
//                                if (targetChunk.Block[xSelection, ySelection] == 0)
//                                {
//                                    targetChunk.ApplyBlockUpdate(new TileUpdate
//                                    {
//                                        X = (byte) xSelection,
//                                        Y = (byte) ySelection,
//                                        Mode = true,
//                                        TileValue = (ushort) WorldTile.Dirt
//                                    });
//                                }
//                            }
//                            else
//                            {//Break block
//                                if (targetChunk.Block[xSelection, ySelection] != 0)
//                                {
//                                    targetChunk.ApplyBlockUpdate(new TileUpdate
//                                    {
//                                        X = (byte) xSelection,
//                                        Y = (byte) ySelection,
//                                        Mode = true,
//                                        TileValue = (ushort) WorldTile.Air
//                                    });
//
//                                    int blockX =
//                                        ((simulation.World.WorldOptions.ChunkSize * chunkInformation.Item1) +
//                                         xSelection) *
//                                        simulation.World.WorldOptions.WorldScale;
//                                    int blockY =
//                                        ((simulation.World.WorldOptions.ChunkSize * chunkInformation.Item2) +
//                                         ySelection) *
//                                        simulation.World.WorldOptions.WorldScale;
//
//                                    #region Create broken block Entity
//                                    
//                                    simulation.AddEntity(new SimulationEntity(
//                                        Guid.NewGuid().ToString(),
//                                        new EntityOptions
//                                        {
//                                            TextureKey = "Player",
//                                            Position = new BfbVector(blockX, blockY),
//                                            Dimensions = new BfbVector(1 * simulation.World.WorldOptions.WorldScale,
//                                                1 * simulation.World.WorldOptions.WorldScale),
//                                            Rotation = 0,
//                                            Origin = new BfbVector(0, 0),
//                                            EntityType = EntityType.Item
//                                        }, new ComponentOptions
//                                        {
//                                            Physics = new TilePhysicsComponent(),
//                                            Input = null,
//                                            GameComponents = new List<ISimulationComponent>
//                                            {
//                                                new LifetimeComponent(1000)
//                                            }
//                                        }));
//                                    
//                                    #endregion
//                                }
//                            }
//                        }
//                    }
//                    
//                    #endregion
//                }

#endregion
             
        }
    }
}