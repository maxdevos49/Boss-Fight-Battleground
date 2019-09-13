//C#
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace BFB.Engine.Event
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
           
            int id = EventHandelerId++;

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


        public void Emit(string eventKey, Event eventData = null)
        {

            //All fired events
            Console.WriteLine($"EventKey: \"{eventKey}\", Mouse X/Y: {eventData?.Mouse?.X},{eventData?.Mouse?.Y}, Key: {eventData?.Keyboard?.Key}");

            if (EventHandelers.ContainsKey(eventKey))
            {
                //create new thread
                Thread eventThread = new Thread(() => EventThread(eventKey, eventData));

                //start thread
                eventThread.Start();

                //EventThread(eventKey, eventData);
            }
        }

        /**
         * Event processing thread.
         * */
        private void EventThread(string eventKey, Event eventData = null)
        {
            if (eventData == null)
            {
                eventData = new Event();
            }

            eventData.EventId = (EventId++);
            eventData.EventKey = eventKey;

            //loop through existing handleres for that event
            foreach (var eventHandeler in EventHandelers[eventKey].ToList())
            {
                //Check if propagation was canceled
                if (eventData.Propagate())
                {
                    //Call event handler
                    eventHandeler.Value(eventData);

                    //Event log
                    //Console.WriteLine($"EventId: {eventData.EventId}, EventKey: \"{eventData.EventKey}\", EventHandelerId: {handeler.Key}");
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
