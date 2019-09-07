using System;


namespace ClientExperiments.Engine.Event
{
    public class Event
    {

        public int EventId { get; set; }

        public string EventKey { get; set; }

        public EventPayload Payload { get; set; }

        private bool PropagateEvent { get; set; }


        #region Constructor

        public Event()
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
