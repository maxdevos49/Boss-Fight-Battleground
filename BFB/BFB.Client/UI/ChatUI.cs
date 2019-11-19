using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class ChatUI : UILayer
    {
        #region Properties

        private List<string> _displayedMessages;

        private ChatModel _chatModel;

        #endregion

        #region Constructor

        public ChatUI() : base(nameof(ChatUI))
        {
            _displayedMessages = new List<string>();
        }

        #endregion

        #region Init

        protected override void Init()
        {
            _chatModel = new ChatModel(ParentScene.Client, this)
            {
                TextBoxText = ""
            };
            AddGlobalListener("Chat", e => { });
        }

        #endregion

        #region Body

        public override void Body()
        {
            Debug = false;

            Color grayBackground = new Color(0, 0, 0, 100);

            RootUI.Zstack(z1 =>
            {
                z1.Vstack(v1 =>
                {
                    v1.Spacer(3);

                    v1.Hstack(h1 =>
                    {
                        h1.Spacer(3);

                        h1.Vstack(v2 =>
                        {
                            _displayedMessages = _chatModel.GetRecent();

                            // Add spacers when < MaxChatsDisplayed displayed
                            for (int i = _displayedMessages.Count; i < ChatModel.MaxChatsDisplayed; i++)
                                v2.Spacer();

                            // Add text components for the most recent (MaxChatsDisplayed) chats
                            foreach (string chat in _displayedMessages)
                                v2.Text(chat).Background(grayBackground);

                            v2.TextBoxFor(_chatModel, t => t.TextBoxText, keyPressAction: (e, a) =>
                            {
                                if (e.Keyboard.KeyEnum != Keys.Enter) return;

                                _chatModel.Send(_chatModel.TextBoxText);
                                _chatModel.TextBoxText = "";
                            }).Background(grayBackground);
                        }).Grow(2);
                    }).Grow(2);
                });
            });
        }

        #endregion
    }

    public class ChatModel
    {
        #region Properties

        public string TextBoxText { get; set; }

        private List<ChatMessage> _chats;

        private readonly ClientSocketManager _client;

        private readonly UILayer _layer;

        public const int MaxChatsDisplayed = 6;

        #endregion

        #region Constructor

        public ChatModel(ClientSocketManager client, UILayer layer)
        {
            _client = client;
            _layer = layer;

            _chats = new List<ChatMessage>();

            Receive();
        }

        #endregion

        #region Send

        public void Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            _client.Emit("Chat", new DataMessage {Message = message});
        }

        #endregion

        #region Receive

        private void Receive()
        {
            _client.On("Chat", (m) =>
            {
                _chats.Add(new ChatMessage(m.Message));
                _layer.Rebuild = true;
            });
        }

        #endregion

        #region GetRecent

        public List<string> GetRecent()
        {
            List<string> messages = new List<string>();

            if (_chats.Count > MaxChatsDisplayed)
                _chats = _chats.GetRange(_chats.Count - MaxChatsDisplayed, MaxChatsDisplayed);

            foreach (ChatMessage chat in _chats)
            {
                if (chat.Timer.ElapsedMilliseconds > 7000)
                    continue;
                
                messages.Add(chat.Message);
            }

            return messages;
        }

        #endregion
        
        #region ChatMessage (class)
        
        private class ChatMessage
        {
            #region Properties
            public string Message { get; set; }

            public Stopwatch Timer { get; }
            
            #endregion

            #region Constructor
            
            public ChatMessage(string message)
            {
                Message = message;
            
                Timer = new Stopwatch();
            
                Timer.Start();
            }
            
            #endregion
        }
        
        #endregion
    }


}