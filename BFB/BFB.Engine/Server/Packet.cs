//C#
using System;

namespace BFB.Engine.Server
{
    [Serializable]
    public class Packet
    {

        /**
         * Target endpoint
         * */
        public string Route { get; set; }

        /**
         * Payload
         * */
        public byte[] Data { get; set; }
    }
}
