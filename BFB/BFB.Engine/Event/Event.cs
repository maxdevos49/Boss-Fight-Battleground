namespace BFB.Engine.Event
{
    public abstract class Event
    {

        public int EventId { get; set; }

        public string EventKey { get; set; }

        private bool PropagateEvent { get; set; }

        #region Constructor

        protected Event()
        {
            PropagateEvent = true;
        }

        #endregion

        #region Propagate

        public bool Propagate()
        {
            return PropagateEvent;
        }

        #endregion

        #region StopPropagation

        public void StopPropagation()
        {
            PropagateEvent = false;
        }

        #endregion

    }
}
