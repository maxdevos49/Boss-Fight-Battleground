//C#
using System;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class DataMessage
    {

        /**
         * Target endpoint
         * */
        public string Route { get; set; }
        
        /**
         * Message to send
         */
        public string Message { get; set; }
        
        /**
         * Client Id
         */
        public string ClientId { get; set; }
        
    }
}
