using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity
{
    public class Entity
    {
        #region Properties
        
        public string EntityId { get; set; }
        
        public BfbVector Position { get; set; }
        
        public BfbVector Dimensions { get; set; }
        
        public BfbVector Origin { get; set; }
        
        public BfbVector Velocity { get; set; }
        
        public float Rotation { get; set; }
        
        #endregion

        #region Constructor
        
        public Entity(string entityId, EntityOptions options)
        {
            EntityId = entityId;
            Position = options.Position;
            Dimensions = options.Dimensions;
            Origin = options.Origin;
            Rotation = options.Rotation;
            Velocity = new BfbVector();
        }
        
        #endregion
        
        #region GetState
        
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
        
        #endregion
    }
    
    #region EntityOptions
    
    public class EntityOptions
    {
        public BfbVector Position { get; set; }
        public BfbVector Dimensions { get; set; }
        public BfbVector Origin { get; set; }
        public float Rotation { get; set; }
    }
    
    #endregion
}