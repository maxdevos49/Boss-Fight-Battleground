using System;
using BFB.Engine.Content;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class EntityMessage : DataMessage
    {
         public string EntityId { get; set; }

         public string AnimationTextureKey { get; set; }
         
         public AnimationState AnimationState { get; set; }
         
         public BfbVector Velocity { get; set; }
         
        public BfbVector Position { get; set; }
        
        public BfbVector Dimensions { get; set; }
        
        public BfbVector Origin { get; set; }
        
        public float Rotation { get; set; }

        public bool Grounded { get; set; }
        
    }
}