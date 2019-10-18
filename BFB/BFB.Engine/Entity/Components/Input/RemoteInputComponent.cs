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

        private PlayerState _playerState;
        
        #endregion

        public RemoteInputComponent(ClientSocket socket)
        {
            _lock = new object();
            _playerState = new PlayerState();
            
            socket.On("/player/input", (m) =>
            {
                InputMessage mm = (InputMessage) m;
                lock (_lock)
                {
                    _playerState = mm.PlayerInputState;//Update new position
                }
            });
            
        }

        public void Update(ServerEntity serverEntity)
        {
            lock (_lock)
            {
                
                //Resets the player movement
                serverEntity.DesiredVector.X = 0;
                serverEntity.DesiredVector.Y = 0;
                //Moves player left
                if (_playerState.Left)
                {
                    serverEntity.DesiredVector.Add(new BfbVector(-1,0));
                }
                //Moves player right
                if (_playerState.Right)
                {
                    serverEntity.DesiredVector.Add(new BfbVector(1,0));
                }
                //Moves player up
                if (_playerState.Jump)
                {
                    serverEntity.DesiredVector.Add(new BfbVector(0,1));
                }
                //serverEntity.DesiredVector = BfbVector.Sub(serverEntity.DesiredVector, serverEntity.Position);
            }
        }
    }
}