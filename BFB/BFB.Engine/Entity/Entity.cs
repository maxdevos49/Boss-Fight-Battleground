using BFB.Engine.Content;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity
{
    public class Entity
    {
        #region Properties
        
        public string ChunkKey { get; set; }
        
        public string EntityId { get; set; }

        public string AnimatedTextureKey { get; set; }
        
        public AnimationState AnimationState { get; set; }
        
        public BfbVector Position { get; set; }
        
        public BfbVector Dimensions { get; set; }
        
        public BfbVector Origin { get; set; }
        
        public BfbVector Velocity { get; set; }
        
        public float Rotation { get; set; }
        
        public bool Grounded { get; set; }

        
        #endregion

        #region Constructor

        protected Entity(string entityId, EntityOptions options)
        {
            EntityId = entityId;
            AnimatedTextureKey = options.AnimatedTextureKey;
            AnimationState = AnimationState.IdleRight;
            Position = options.Position;
            Dimensions = options.Dimensions;
            Origin = options.Origin;
            Rotation = options.Rotation;
            Velocity = new BfbVector();
            Grounded = false;
        }
        
        #endregion
        
        #region GetState
        
        public EntityMessage GetState()
        {
            return new EntityMessage
            {
                EntityId = EntityId,
                ChunkKey = ChunkKey,
                AnimationTextureKey = AnimatedTextureKey,
                AnimationState = AnimationState,
                Position = Position,
                Dimensions = Dimensions,
                Origin = Origin,
                Rotation = Rotation,
                Velocity = Velocity,
                Grounded = Grounded
            };
        }
        
        #endregion
    }
    
    #region EntityOptions
    
    public class EntityOptions
    {
        public string AnimatedTextureKey { get; set; }
        public BfbVector Position { get; set; }
        public BfbVector Dimensions { get; set; }
        public BfbVector Origin { get; set; }
        public float Rotation { get; set; }
    }
    
    #endregion
}