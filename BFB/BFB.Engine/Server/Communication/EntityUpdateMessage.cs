using System;
using System.Collections.Generic;

namespace BFB.Engine.Server.Communication
{
    /// <summary>
    /// Used to pass a group if entity update messages to the client
    /// </summary>
    [Serializable]
    public class EntityUpdateMessage : DataMessage
    {
        /// <summary>
        /// Constructs a EntityUpdateMessage
        /// </summary>
        public EntityUpdateMessage()
        {
            Updates = new List<EntityMessage>();
        }
        
        /// <summary>
        /// List of entity updates to give to the client
        /// </summary>
        public List<EntityMessage> Updates { get; set; }
    }
}