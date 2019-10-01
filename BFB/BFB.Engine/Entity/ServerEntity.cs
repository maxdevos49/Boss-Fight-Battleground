//Monogame

using System;
using Microsoft.Xna.Framework;

//Engine
using BFB.Engine.Entity.Components.Graphics;
using BFB.Engine.Entity.Components.Input;
using BFB.Engine.Entity.Components.Physics;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;

namespace BFB.Engine.Entity
{
    public class ServerEntity : Entity
    {
        
        #region Properties
        public BfbVector DesiredVector { set; get; }
        
        #endregion
        
        #region Components
        
        private readonly IInputComponent _input;
        private readonly IPhysicsComponent _physics;
        private readonly IGraphicsComponent _graphics;
        private AnimationComponent _animation;
        
        #endregion

        public ServerEntity(string entityId, EntityOptions options, ComponentOptions components)
        {
            EntityId = entityId;
            
            //Options
            Position = options.Position;
            Dimensions = options.Dimensions;
            Origin = options.Origin;
            Rotation = options.Rotation;
            Velocity = new BfbVector();
            
            //Components
            _input = components.Input;
            _graphics = components.Graphics;
            _physics = components.Physics;
            _animation = components.Animation;
        }

        public void Update()
        {
            _input?.Update(this);
            _physics?.Update(this);
            _animation.Update(this);
        }

        public void Draw()
        {
            _graphics?.Draw(this);
        }

        public EntityMessage GetState()
        {
            return new EntityMessage
            {
                EntityId = EntityId,
                Position = Position,
                Dimensions = Dimensions,
                Origin = Origin,
                Rotation = Rotation,
                Velocity = Velocity
            };
        }
       
    }
    
    #region EntityOptions
    
    public class ComponentOptions
    {
        public IInputComponent Input { get; set; }
        public IPhysicsComponent Physics { get; set; }
        public IGraphicsComponent Graphics { get; set; }
        public AnimationComponent Animation { get; set; }
    }

    public class EntityOptions
    {
        public BfbVector Position { get; set; }
        public BfbVector Dimensions { get; set; }
        public BfbVector Origin { get; set; }
        public float Rotation { get; set; }
    }
    
    #endregion
    
}