using System;
using System.Collections.Generic;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class EntityUpdateMessage : DataMessage
    {
        public EntityUpdateMessage()
        {
            Updates = new List<EntityMessage>();
        }
        public List<EntityMessage> Updates { get; set; }
    }
}