//C#
using System;

namespace BFB.Engine.Server.Communication
{
    /// <summary>
    /// A Base Data Structure to communicate between the client and server
    /// </summary>
    [Serializable]
    public class DataMessage
    {

        /// <summary>
        /// Message route or endpoint
        /// </summary>
        public string Route { get; set; }
        
        /// <summary>
        /// A string message for passing information
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// The sending or receiving client id
        /// </summary>
        public string ClientId { get; set; }
        
    }
}
