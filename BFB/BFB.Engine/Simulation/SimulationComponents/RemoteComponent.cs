using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents.AI;
using BFB.Engine.Simulation.SimulationComponents.Combat;

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
            
            //Add an AI monster//TODO
                if (entity.ControlState.HotBarRight)
                {

                    var random = new Random();
                    int type = random.Next(0, 3);
                    string textureKey = "";
                    int xMaxSpeed = 20;
                    BfbVector dimensions = new BfbVector(2* simulation.World.WorldOptions.WorldScale,3* simulation.World.WorldOptions.WorldScale);
                    
                    switch (type)
                    {
                        case 0://skeleton
                            textureKey = "Skeleton";
                            xMaxSpeed = 18;
                            break;
                        case 1://zombie
                            textureKey = "Zombie";
                            xMaxSpeed = 15;
                            break;
                        case 2://spider
                            textureKey = "Spider";
                            xMaxSpeed = 17;
                            dimensions = new BfbVector(3* simulation.World.WorldOptions.WorldScale,2* simulation.World.WorldOptions.WorldScale);
                            break;
                    }
                    
                    //Add to simulation
                    simulation.AddEntity(new SimulationEntity(
                        Guid.NewGuid().ToString(),
                        new EntityOptions
                        {
                            TextureKey = textureKey,
                            Position = new BfbVector(entity.ControlState.Mouse.X, entity.ControlState.Mouse.Y),
                            Dimensions = dimensions,
                            Rotation = 0,
                            Origin = new BfbVector(0, 0),
                            EntityType = EntityType.Mob
                        }, 
                         new List<SimulationComponent>
                        {
                            new WalkingAnimationComponent(),
                            new WalkingPhysics(xMaxSpeed),
                            new AIInputComponent(),
                            new LifetimeComponent(2000),
                            new CombatComponent()
                        }
                        ));
                }

            #endregion

        }
    }

    public interface ISimulationComponent
    {
    }
}