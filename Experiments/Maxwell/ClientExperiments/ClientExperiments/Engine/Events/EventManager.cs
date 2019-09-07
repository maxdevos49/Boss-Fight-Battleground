using System;
using System.Collections.Generic;

namespace ClientExperiments.Engine.Event
{
    public class EventManager
    {
        //Dictionary<string:EventKey, Dictionary<int:HandelerId, Func<IEvent>>> EventHandelers;
        private readonly Dictionary<string, Dictionary<int, Action<Event>>> EventHandelers;

        private int EventHandelerId;
        private int EventId;

        public EventManager()
        {
            EventHandelerId = 0;
            EventId = 0;
            EventHandelers = new Dictionary<string, Dictionary<int, Action<Event>>>();
        }


        /**
         * Adds a event listener for a specified event
         * */
        public int AddEventListener(string eventKey, Action<Event> eventCallback)
        {
            //get handlerId
            EventHandelerId++;
            int id = EventHandelerId;

            //add event handeler
            if (EventHandelers.ContainsKey(eventKey))
            {
                //event type already exist
                EventHandelers[eventKey].Add(id, eventCallback);
            }
            else
            {
                //event type does not exist so add it
                EventHandelers.Add(eventKey, new Dictionary<int, Action<Event>>());

                //add event handeler
                EventHandelers[eventKey].Add(id, eventCallback);
            }

            return id;
        }


        /**
         * Removes a event listener for a specified event handler id
         * */
        public void RemoveEventListener(int eventHandlerId)
        {
            foreach (var eventType in EventHandelers)
            {

                //check each event type
                if (eventType.Value.ContainsKey(eventHandlerId))
                {
                    //remove the handeler
                    EventHandelers[eventType.Key].Remove(eventHandlerId);
                    break;
                }
            }
        }


        public void Emit(string eventKey, EventPayload payload = null)
        {

            Event eventObj = new Event
            {
                EventId = (EventId++),
                EventKey = eventKey,
                Payload = payload
            };

            //make sure the event key exist
            if (EventHandelers.ContainsKey(eventKey))
            {
                //loop through existing handleres for that event
                foreach (var handeler in EventHandelers[eventKey])
                {

                    //Check if propagation was canceled
                    if (eventObj.Propagate())
                    {
                        //Call event handler
                        handeler.Value(eventObj);
                    }
                    else
                    {
                        //event was canceled
                        break;
                    }
                }
            }
        }


    }
}
