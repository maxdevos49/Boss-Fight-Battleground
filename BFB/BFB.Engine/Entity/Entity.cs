using BFB.Engine.Math;

namespace BFB.Engine.Entity
{
    public class Entity
    {
        public string EntityId { get; set; }
        
        public BfbVector Position { get; set; }
        
        public BfbVector Dimensions { get; set; }
        
        public BfbVector Origin { get; set; }
        
        public BfbVector Velocity { get; set; }
        
        public float Rotation { get; set; }
    }
}