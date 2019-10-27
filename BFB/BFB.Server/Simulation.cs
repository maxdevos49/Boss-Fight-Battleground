using System;
using System.Collections.Generic;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;

namespace BFB.Server
{
    public class Simulation
    {

        #region Properties
        
        private readonly object _lock;
        private readonly Dictionary<string, ServerEntity> _entities;
        private readonly ServerSocketManager _server;
        private readonly int _tickSpeed;
        private bool _running;

        #endregion
        
        #region Constructor
        
        /**
         * Thread safe simulation class that can be ticked to move the simulation forward a single step at a time
         */
        public Simulation(ServerSocketManager server, int? tickSpeed = null)
        {
            _lock = new object();
            _server = server;
            _entities = new Dictionary<string, ServerEntity>();
            _running = false;
            _tickSpeed = tickSpeed ?? (1000 / 60);//60 ticks a second are default
        }
        
        #endregion

        #region AddEntity
        
        public void AddEntity(ServerEntity serverEntity)
        {
            lock (_lock)
            {
                _entities.Add(serverEntity.EntityId, serverEntity);
            }
        }

        #endregion
        
        #region RemoveEntity
        
        public void RemoveEntity(string key)
        {
            lock (_lock)
            {
                if(_entities.ContainsKey(key))
                    _entities.Remove(key);
            }

            _server.Emit("/player/disconnect", new DataMessage {Message = key});
        }
        
        #endregion
        
        #region Start

        public void Start()
        {
            _running = true;
            Thread t = new Thread(Simulate)
            {
                Name = "Simulation",
                IsBackground = true
            };
            t.Start();
        }
        
        #endregion   
        
        #region Stop

        public void Stop()
        {
            _running = false;
        }
        
        #endregion
        
        #region GetUpdates

        [UsedImplicitly]
        public EntityUpdateMessage GetUpdates()
        {
            lock (_lock)
            {
                EntityUpdateMessage updates = new EntityUpdateMessage();

                foreach ((string key, ServerEntity entity) in _entities)
                {
                    updates.Updates.Add(entity.GetState());
                }
                
                return updates;
            }
        }

        #endregion

        #region Simulate
        
        private void Simulate()
        {
            long nextTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //Server Game loop
            while (_running)
            {
                //Ask for input from all players
                _server.Emit("/players/getUpdates");
                
                lock (_lock)
                {
                    //Update entities
                    foreach ((string key, ServerEntity entity) in _entities)
                    {
                        entity.Tick( /*Pass in world in the future*/);
                    }
                
                    //Send changes. In future cull updates per player to reduce sending un needed data to some clients(because that thing may not be on there screen)
                    if(_entities.Count > 0){
                        _server.Emit("/players/updates", GetUpdates());
                    }
                    
                    //TODO In future tick chunks also for dynamic tiles(Fire, gravity updates, grass)
                }
                

                //Maintain the tick rate here
                nextTick += _tickSpeed;
                int sleepTime = (int) (nextTick - (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));
                if (sleepTime >= 0)
                {
                    Thread.Sleep(sleepTime);
                }
                else
                {
                    _server.PrintMessage($"SERVER IS OVERLOADED. ({sleepTime}TPS).");
                }
            }
        }
        
        #endregion
        
    }
}