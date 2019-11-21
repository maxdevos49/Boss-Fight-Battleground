using BFB.Engine.Content;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;

namespace BFB.Engine.Entity
{
    /// <summary>
    /// An entity that exists in the game
    /// </summary>
    public class Entity
    {
        #region Properties
        
        public string ChunkKey { get; set; }
        
        public string EntityId { get; set; }

        /// <summary>
        /// Key to which texture to apply
        /// </summary>
        public string TextureKey { get; set; }
        
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
        /// The type of entity the entity is
        /// </summary>
        public EntityType EntityType { get; set; }
        
        /// <summary>
        /// The direction the entity is facing
        /// </summary>
        public DirectionFacing Facing { get; set; }
        
        /// <summary>
        /// Holds information like health, mana and other things
        /// </summary>
        [CanBeNull]
        public EntityMeta Meta { get; set; }
        
        /// <summary>
        /// If the entity is grounded or not
        /// </summary>
        public bool Grounded { get; set; }
        
        /// <summary>
        /// The width of the entity
        /// </summary>
        public int Width => (int) Dimensions.X;
        
        /// <summary>
        /// The height of the entity
        /// </summary>
        public int Height => (int) Dimensions.Y;
        
        /// <summary>
        /// The bottom position of the player
        /// </summary>
        public int Bottom => (int)(Position.Y + Height);
        
        /// <summary>
        /// The left side of the players position
        /// </summary>
        public int Left => (int)(Position.X);
        
        /// <summary>
        /// The right side of the players position
        /// </summary>
        public int Right => (int)(Position.X + Width);
        
        /// <summary>
        /// The top side of the player
        /// </summary>
        public int Top => (int)(Position.Y);
        
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
            TextureKey = options.TextureKey;
            AnimationState = AnimationState.IdleRight;
            Position = options.Position;
            Dimensions = options.Dimensions;
            Origin = options.Origin;
            Rotation = options.Rotation;
            EntityType = options.EntityType;
            Grounded = false;
            Velocity = new BfbVector();
            Facing = DirectionFacing.Left;
        }
        
        #endregion
        
        #region GetState
        
        /// <summary>
        /// Creates and returns an EntityMessage for this entity
        /// </summary>
        /// <returns>New Entity Message containing the state of this entity's properties</returns>
        public EntityMessage GetState()
        {
            return new EntityMessage
            {
                EntityId = EntityId,
                ChunkKey = ChunkKey,
                TextureKey = TextureKey,
                AnimationState = AnimationState,
                Position = Position,
                Dimensions = Dimensions,
                Origin = Origin,
                Rotation = Rotation,
                Velocity = Velocity,
                Grounded = Grounded,
                Facing = Facing,
                EntityType = EntityType,
                Meta = Meta
            };
        }
        
        #endregion
    }
}