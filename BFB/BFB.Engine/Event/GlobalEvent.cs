namespace BFB.Engine.Event
{
    public class GlobalEvent : Event
    {
        public string Message { get; set; }

        /**
         * Only use if absolutely necessary. If it sees fit add a strongly typed property instead
         * of this. object encourages messy programming problems and is lacking of type safety due
         * to need of cast to use the useful stuff inside of it
         */
        public object Data { get; set; }
    }
}