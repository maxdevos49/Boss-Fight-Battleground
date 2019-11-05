namespace BFB.Engine.Event
{
    /// <summary>
    /// An event to be send globally
    /// </summary>
    public class GlobalEvent : Event
    {
        /// <summary>
        /// Message to be sent
        /// </summary>
        public string Message { get; set; }

        
         /// <summary>
         /// Only use if absolutely necessary. If it sees fit add a strongly typed property instead
         /// of this. object encourages messy programming problems and is lacking of type safety due
         /// to need of cast to use the useful stuff inside of it
         /// </summary>
        public object Data { get; set; }
    }
}