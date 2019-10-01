//C#
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace BFB.Engine.Event
{
    public class EventManager
    {
        //Dictionary<string:EventKey, Dictionary<int:HandlerId, Func<IEvent>>> EventHandlers;
        private readonly Dictionary<string, Dictionary<int, Action<Event>>> EventHandlers;

        private int EventHandlerId;
        private int EventId;

        private Queue<Event> EventQueue;

        public EventManager()
        {
            EventHandlerId = 0;
            EventId = 0;
            EventHandlers = new Dictionary<string, Dictionary<int, Action<Event>>>();
            EventQueue = new Queue<Event>();
        }


        /**
         * Adds an event listener for a specified event
         * */
        public int AddEventListener(string eventKey, Action<Event> eventCallback)
        {
            //get handlerId
            int id = EventHandlerId++;

            //add event handler
            if (EventHandlers.ContainsKey(eventKey))
            {
                //event type already exist
                EventHandlers[eventKey].Add(id, eventCallback);
            }
            else
            {
                //event type does not exist so add it
                EventHandlers.Add(eventKey, new Dictionary<int, Action<Event>>());

                //add event handler
                EventHandlers[eventKey].Add(id, eventCallback);
            }

            return id;
        }


        /**
         * Removes a event listener for a specified event handler id
         * */
        public void RemoveEventListener(int eventHandlerId)
        {
            foreach (var eventType in EventHandlers)
            {

                //check each event type
                if (eventType.Value.ContainsKey(eventHandlerId))
                {
                    //remove the handler
                    EventHandlers[eventType.Key].Remove(eventHandlerId);
                    break;
                }
            }
        }


        public void Emit(string eventKey, Event eventData = null)
        {
            //All fired events
            Console.WriteLine($"EventKey: \"{eventKey}\", Mouse X/Y: {eventData?.Mouse?.X},{eventData?.Mouse?.Y}, Key: {eventData?.Keyboard?.Key}");
            
            if (eventData == null)
                eventData = new Event();

            eventData.EventId = (EventId++);
            eventData.EventKey = eventKey;

            if (EventHandlers.ContainsKey(eventKey))
                EventQueue.Enqueue(eventData);
        }

        /**
         * Processes the events in the EventQueue
         * */
        public void ProcessEvents()
        {
            while (EventQueue.Count > 0)
            {
                Event currentEvent = EventQueue.Dequeue();
                
                //loop through existing handlers for that event
                foreach (var eventHandler in EventHandlers[currentEvent.EventKey].ToList())
                {
                    //Check if propagation was canceled
                    if (currentEvent.Propagate())
                    {
                        //Call event handler
                        eventHandler.Value(currentEvent);

                        //Event log
                        //Console.WriteLine($"EventId: {eventData.EventId}, EventKey: \"{eventData.EventKey}\", EventHandlerId: {handler.Key}");
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
