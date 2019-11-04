namespace BFB.Engine.Event
{
    public abstract class Event
    {
        /// <summary>
        /// Unique ID of this event
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Key to this event
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// To propagate this event or not
        /// </summary>
        private bool PropagateEvent { get; set; }

        #region Constructor

        /// <summary>
        /// Creates a new event 
        /// </summary>
        protected Event()
        {
            PropagateEvent = true;
        }

        #endregion

        #region Propagate

        /// <summary>
        /// Whether to propagate this event
        /// </summary>
        /// <returns>This event's PropagateEvent property</returns>
        public bool Propagate()
        {
            return PropagateEvent;
        }

        #endregion

        #region StopPropagation

        /// <summary>
        /// Stops propagation of this event
        /// </summary>
        public void StopPropagation()
        {
            PropagateEvent = false;
        }

        #endregion

    }
}
