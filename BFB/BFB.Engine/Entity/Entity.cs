using BFB.Engine.Content;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity
{
    /// <summary>
    /// An entity that exists in the game
    /// </summary>
    public class Entity
    {
        #region Properties
        
        /// <summary>
        /// Unique ID
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Key to which texture to apply
        /// </summary>
        public string AnimatedTextureKey { get; set; }
        
        /// <summary>
        /// Current state of the animation
        /// </summary>
        public AnimationState AnimationState { get; set; }
        
        /// <summary>
        /// Current position of this entity on the map
        /// </summary>
        public BfbVector Position { get; set; }
        
        /// <summary>
        /// Vector of dimensions for this entity
        /// </summary>
        public BfbVector Dimensions { get; set; }
        
        /// <summary>
        /// Point that this entity calculates position and rotation from
        /// </summary>
        public BfbVector Origin { get; set; }
        
        /// <summary>
        /// Speed and direction of movement for this entity
        /// </summary>
        public BfbVector Velocity { get; set; }
        
        /// <summary>
        /// The rotation of this entity
        /// </summary>
        public float Rotation { get; set; }
        
        /// <summary>
        /// Whether this entity is on the ground or not
        /// </summary>
        public bool Grounded { get; set; }

        
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Entity
        /// </summary>
        /// <param name="entityId">Unique ID for this entity</param>
        /// <param name="options">Options object to be applied to this entity</param>
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
        
        /// <summary>
        /// Gets the state of this entity's 
        /// </summary>
        /// <returns></returns>
        public EntityMessage GetState()
        {
            return new EntityMessage
            {
                EntityId = EntityId,
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