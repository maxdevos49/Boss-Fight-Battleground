//C#
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

//Jetbrains
using JetBrains.Annotations;

namespace BFB.Engine.Event
{
    public class EventManager
    {
        //Dictionary<string:EventKey, Dictionary<int:HandlerId, Func<IEvent>>> EventHandlers;
        private readonly Dictionary<string, Dictionary<int, Action<Event>>> _eventHandlers;

        private int _eventHandlerId;
        private int _eventId;

        public EventManager()
        {
            _eventHandlerId = 0;
            _eventId = 0;
            _eventHandlers = new Dictionary<string, Dictionary<int, Action<Event>>>();
        }


        /**
         * Adds a event listener for a specified event
         * */
        [UsedImplicitly]
        public int AddEventListener(string eventKey, Action<Event> eventCallback)
        {
            //get handlerId
            int id = _eventHandlerId++;

            //add event handler
            if (_eventHandlers.ContainsKey(eventKey))
            {
                //event type already exist
                _eventHandlers[eventKey].Add(id, eventCallback);
            }
            else
            {
                //event type does not exist so add it
                _eventHandlers.Add(eventKey, new Dictionary<int, Action<Event>>());

                //add event handler
                _eventHandlers[eventKey].Add(id, eventCallback);
            }

            return id;
        }


        /**
         * Removes a event listener for a specified event handler id
         * */
        [UsedImplicitly]
        public void RemoveEventListener(int eventHandlerId)
        {
            foreach (KeyValuePair<string, Dictionary<int, Action<Event>>> eventType in _eventHandlers.Where(eventType => eventType.Value.ContainsKey(eventHandlerId)))
            {
                //remove the handler
                _eventHandlers[eventType.Key].Remove(eventHandlerId);
                break;
            }
        }


        public void Emit(string eventKey, Event eventData = null)
        {
            if (!_eventHandlers.ContainsKey(eventKey)) return;
            
            //create new thread
            Thread eventThread = new Thread(() => EventThread(eventKey, eventData))
            {
                IsBackground = true
            };

            //start thread
            eventThread.Start();
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

            eventData.EventId = (_eventId++);
            eventData.EventKey = eventKey;

            //loop through existing handler's for that event
            foreach (KeyValuePair<int, Action<Event>> eventHandler in _eventHandlers[eventKey].ToList())
            {
                //Check if propagation was canceled
                if (eventData.Propagate())
                {
                    //Call event handler
                    eventHandler.Value(eventData);
                }
                else
                {
                    break;
                }
            }
        }
    }
}