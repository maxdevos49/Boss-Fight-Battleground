using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace BFB.Engine.Server
{
    public static class PacketManager
    {

        #region SerializeObjectIntoPacket
        /**
         * Serializes a object and places it in a new message object
         * */
        public static Packet Serialize(object anySerializableObject)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, anySerializableObject);
                return new Packet { Data = memoryStream.ToArray() };
            }
        }

        #endregion

        #region DeserializeObjectFromPacket

        /**
         * Deserializes a message object Data property after being sent over a tcp connection
         * */
        public static object Deserialize(Packet message)
        {

            using (MemoryStream memoryStream = new MemoryStream(message.Data))
                return new BinaryFormatter().Deserialize(memoryStream);
        }

        #endregion

        #region WritePacketToStream

        public static void Write(NetworkStream stream, Packet message)
        {
            byte[] messageData;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, message);
                messageData = memoryStream.ToArray();
            }

            //get and convert length to bytes
            byte[] messageLength = BitConverter.GetBytes(messageData.Length);

            //send message length
            stream.Write(messageLength, 0, 4);

            //send message
            stream.Write(messageData, 0, messageData.Length);
        }

        #endregion

        #region ReadPacketFromStream

        public static Packet Read(NetworkStream stream, int messageSize)
        {
            byte[] messageData = new byte[messageSize];

            //Safe read
            int bytesRead = 0;
            do
            {
                Console.WriteLine($"Byte position: {bytesRead}");
                bytesRead += stream.Read(messageData, bytesRead, messageSize - bytesRead);
            } while (bytesRead < messageSize);

            Console.WriteLine($"Total bytes Read: {bytesRead}");

            //do something with message now
            using (MemoryStream memoryStream = new MemoryStream(messageData))
                return (Packet)new BinaryFormatter().Deserialize(memoryStream);
        }

        #endregion

    }
}
