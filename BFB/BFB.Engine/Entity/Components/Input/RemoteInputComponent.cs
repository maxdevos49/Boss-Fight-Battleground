using System;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Input
{
    public class RemoteInputComponent : IInputComponent
    {
        
        #region Properties

        private readonly object _lock;

        private BfbVector _mouse;
        
        #endregion

        public RemoteInputComponent(ClientSocket socket)
        {
            _lock = new object();
            _mouse = new BfbVector();
            
            socket.On("/player/input", (m) =>
            {
                InputMessage mm = (InputMessage) m;
                lock (_lock)
                {
                    _mouse = mm.MousePosition;//Update new position
                }
            });
            
        }

        public void Update(ServerEntity serverEntity)
        {
            lock (_lock)
            {
                serverEntity.DesiredVector = BfbVector.Sub(_mouse, serverEntity.Position);
            }
        }
    }
}