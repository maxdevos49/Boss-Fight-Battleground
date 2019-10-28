//Monogame

using System;
using Microsoft.Xna.Framework;

//Engine
using BFB.Engine.Entity.Components.Graphics;
using BFB.Engine.Entity.Components.Input;
using BFB.Engine.Entity.Components.Physics;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using BFB.Engine.TileMap;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Graphics;

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
        
        #endregion

        public ServerEntity(string entityId, EntityOptions options, ComponentOptions components) : base(entityId, options)
        {
            //Components
            _input = components.Input;
            _physics = components.Physics;
            
            DesiredVector = new BfbVector();
        }

        #region Update
        
        public void Tick(Chunk[,] chunks)
        {
            _input?.Update(this);
            _physics?.Update(this, chunks);
        }
        
        #endregion

    }
    
    #region ComponentOptions
    
    public class ComponentOptions
    {
        public IInputComponent Input { get; set; }
        public IPhysicsComponent Physics { get; set; }
    }
    
    
    #endregion
    
}