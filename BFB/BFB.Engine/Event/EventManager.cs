//C#
using System;
using System.Linq;
using System.Collections.Generic;

//Jetbrains
using JetBrains.Annotations;

namespace BFB.Engine.Event
{
    /// <summary>
    /// Manager for events.
    /// Devides what is to be done when an event is fired
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public class EventManager<TEvent> where TEvent : Event, new()
    {
        private readonly Dictionary<string, Dictionary<int, Action<TEvent>>> _eventHandlers;
        public Func<TEvent,bool> OnEventProcess { get; set; }

        private int _eventHandlerId;
        private int _eventId;

        private readonly Queue<TEvent> _eventQueue;

        public EventManager()
        {
            _eventHandlerId = 0;
            _eventId = 0;
            _eventHandlers = new Dictionary<string, Dictionary<int, Action<TEvent>>>();
            _eventQueue = new Queue<TEvent>();
            OnEventProcess = null;
        }


        /// <summary>
        /// Adds an event listener for a specified event
        /// </summary>
        /// <param name="eventKey">Unique key of the event being used</param>
        /// <param name="eventCallback">Callback that will happen when this event is fired</param>
        /// <returns></returns>
        [UsedImplicitly]
        public int AddEventListener(string eventKey, Action<TEvent> eventCallback)
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
                _eventHandlers.Add(eventKey, new Dictionary<int, Action<TEvent>>());

                //add event handler
                _eventHandlers[eventKey].Add(id, eventCallback);
            }

            return id;
        }


        /// <summary>
        /// Removes a event listener for a specified event handler id
        /// </summary>
        /// <param name="eventHandlerId">Unique ID of the event handler to remove</param>
        [UsedImplicitly]
        public void RemoveEventListener(int eventHandlerId)
        {
            foreach ((string key, Dictionary<int, Action<TEvent>> _) in _eventHandlers.Where(eventType => eventType.Value.ContainsKey(eventHandlerId)))
            {
                //remove the handler
                _eventHandlers[key].Remove(eventHandlerId);
                break;
            }
        }

        /// <summary>
        /// Adds the event with the given key and data to the EventQueue
        /// </summary>
        /// <param name="eventKey">key of the event to be added</param>
        /// <param name="eventData">data of the event to be added</param>
        public void Emit(string eventKey, TEvent eventData = null)
        {
            if (eventData == null)
                eventData = new TEvent();

            eventData.EventId = (_eventId++);
            eventData.EventKey = eventKey;

            _eventQueue.Enqueue(eventData);//This could be a setting to cull events with no listeners
        }

        /// <summary>
        /// Processes the events in the EventQueue
        /// </summary>
        public void ProcessEvents()
        {
            while (_eventQueue.Count > 0)
            {
                TEvent currentEvent = _eventQueue.Dequeue();

                if (!(OnEventProcess?.Invoke(currentEvent) ?? false)) continue;
                

                if(!_eventHandlers.ContainsKey(currentEvent.EventKey)) continue;
                
                //loop through existing handlers for that event
                foreach ((int _, Action<TEvent> eventHandler) in _eventHandlers[currentEvent.EventKey].ToList().TakeWhile(eventHandler => currentEvent.Propagate()))
                {
                    //Call event handler
                    eventHandler(currentEvent);
                }

            }
        }
    }
}