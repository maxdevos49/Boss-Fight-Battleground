using System;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Server.Communication
{
    
    /// <summary>
    /// Used to pass information about the entity to the client
    /// </summary>
    [Serializable]
    public class EntityMessage : DataMessage
    {
        /// <summary>
        /// The entities Id
        /// </summary>
        public string EntityId { get; set; }
        
        public string ChunkKey { get; set; }

        /// <summary>
        /// The animation key that the entity should use
        /// </summary>
        public string TextureKey { get; set; }

        /// <summary>
        /// The Animation state enum that the entity is currently in
        /// </summary>
        public AnimationState AnimationState { get; set; }

        /// <summary>
        /// The velocity of the entity at the time of the current update
        /// </summary>
        public BfbVector Velocity { get; set; }
         
        /// <summary>
        /// The position of the entity at the time of the current update
        /// </summary>
        public BfbVector Position { get; set; }
        
        /// <summary>
        /// The Dimensions of the entity
        /// </summary>
        public BfbVector Dimensions { get; set; }
        
        /// <summary>
        /// The origin of the entity
        /// </summary>
        public BfbVector Origin { get; set; }
        
        /// <summary>
        /// The rotation of the entity
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Indicates if the entity is currently grounded or not
        /// </summary>
        public bool Grounded { get; set; }
        
        /// <summary>
        /// The direction the player is facing
        /// </summary>
        public DirectionFacing Facing { get; set; }
        
        /// <summary>
        /// The type of entity it is
        /// </summary>
        public EntityType EntityType { get; set; }        
    }
}