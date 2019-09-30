using System;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class EntityMessage : DataMessage
    {
         public string EntityId { get; set; }
         
         public BfbVector Velocity { get; set; }
         
        public BfbVector Position { get; set; }
        
        public BfbVector Dimensions { get; set; }
        
        public BfbVector Origin { get; set; }
        
        public float Rotation { get; set; }
    }
}