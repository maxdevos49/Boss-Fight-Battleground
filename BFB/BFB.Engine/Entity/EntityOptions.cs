using BFB.Engine.Math;

namespace BFB.Engine.Entity
{
    /// <summary>
    /// Contains properties of an entity to be applied to an entity. Specifically a new entity while being created.
    /// </summary>
    public class EntityOptions
    {
        /// <summary>
        /// Key to which texture to apply
        /// </summary>
        public string TextureKey { get; set; }
        
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
        /// The rotation of this entity
        /// </summary>
        public float Rotation { get; set; }
    
        /// <summary>
        /// Indicates the type of entity
        /// </summary>
        public EntityType EntityType { get; set; }
        
    }
}