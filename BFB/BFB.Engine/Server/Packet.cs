//C#
using System;

namespace BFB.Engine.Server
{
    [Serializable]
    public class Packet
    {

        /**
         * Identifier in case of multiple games
         * */
        public string Namespace { get; set; }

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
