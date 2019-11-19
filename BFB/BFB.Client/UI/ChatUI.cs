using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private string TextBoxMessage { get; set; }

        private ChatModel _model;

        private Chat _chat;

        #region Constructor

        public ChatUI() : base(nameof(ChatUI))
        {
            _model = new ChatModel() {TextBoxText = ""};
        }

        #endregion

        #region Init

        protected override void Init()
        {
            _chat = new Chat(ParentScene.Client);
            
            AddGlobalListener("Chat", e =>
            {
                TextBoxMessage = e.Message;
            });
        }

        #endregion

        #region Body

        public override void Body()
        {
            Debug = false;

            Color background = new Color(0, 0, 0, 100);

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
                            v2.Text("Test message").Background(background);
                            v2.Text("Test message").Background(background);
                            v2.Text("Test message").Background(background);
                            v2.Text("Test message").Background(background);
                            v2.Text("Test message").Background(background);
                            v2.Text("Test message").Background(background);
                            v2.TextBoxFor(_model, t => t.TextBoxText, keyPressAction: (e, a) =>
                            {
                                if (e.Keyboard.KeyEnum == Keys.Enter)
                                {
                                    _chat.Send(_model.TextBoxText);
                                    _model.TextBoxText = " ";
                                }
                            }).Background(background);
                        }).Grow(2);
                    }).Grow(2);
                });
            });
        }

        #endregion
    }

    public class ChatModel
    {
        public string TextBoxText { get; set; }

        public List<string> Messages;
    }

    public class Chat
    {
        private List<string> Chats;

        private ClientSocketManager _client;

        public Chat (ClientSocketManager client)
        {
            _client = client;
            
            Chats = new List<string>();
            
            Receive();
        }

        public void Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            _client.Emit("Chat", new DataMessage {Message = message});
        }

        private void Receive()
        {
            _client.On("Chat", (m) =>
            {
                Chats.Add(m.Message);
            });
        }

        public List<string> GetRecent()
        {
            return Chats.Count <= 5 ? Chats : Chats.GetRange(Chats.Count - 5, Chats.Count);
        }
    }
}