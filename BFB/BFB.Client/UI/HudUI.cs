using System;
using BFB.Client.Helpers;
using BFB.Engine;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class HudUI : UILayer
    {
        
        private  ClientDataRegistry ClientData { get; set; }

        public HudUI() : base(nameof(HudUI))
        {
            BlockInput = false;
        }

        protected override void Init()
        {
            ClientData = ClientDataRegistry.GetInstance();

            #region Keypress Events

            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                        UIManager.StartLayer(nameof(GameMenuUI),ParentScene);
                        break;
                    case Keys.E:
                        UIManager.LaunchLayer(nameof(InventoryUI), ParentScene);
                        break;
                    case Keys.T:
                        UIManager.StartLayer(nameof(ChatUI), ParentScene);
                        break;
                    case Keys.D1:
                        ClientData.Inventory.ActiveSlot = 0;
                        break;
                    case Keys.D2:
                        ClientData.Inventory.ActiveSlot = 1;
                        break;
                    case Keys.D3:
                        ClientData.Inventory.ActiveSlot = 2;
                        break;
                    case Keys.D4:
                        ClientData.Inventory.ActiveSlot = 3;
                        break;
                    case Keys.D5:
                        ClientData.Inventory.ActiveSlot = 4;
                        break;
                    case Keys.D6:
                        ClientData.Inventory.ActiveSlot = 5;
                        break;
                    case Keys.D7:
                        ClientData.Inventory.ActiveSlot = 6;
                        break;
                }
            });

            #endregion
            
            #region MouseScroll Events
            
            AddInputListener("mousescroll", e =>
            {

                if (Math.Abs(e.Mouse.VerticalScrollAmount) <= 200 || e.Keyboard.KeyboardState.IsKeyDown(Keys.LeftControl))
                    return;
                
                if (e.Mouse.VerticalScrollAmount > 0)
                    ClientData.Inventory.ActiveSlot++;
                else
                {
                    if (ClientData.Inventory.ActiveSlot == 0)
                        ClientData.Inventory.ActiveSlot = 6;
                    else
                        ClientData.Inventory.ActiveSlot--;
                }

                if (ClientData.Inventory.ActiveSlot > 6)
                    ClientData.Inventory.ActiveSlot = 0;

            });
            
            #endregion
        }

        public override void Body()
        {
            RootUI.Background(Color.Transparent);

            RootUI.Zstack(z1 =>
            {
                z1.Vstack(h2 =>
                    {

                        //Health Bar
                        h2.HudMeter(ClientData, x => x.Client.Meta);
                        //Mana Bar
                        h2.HudMeter(ClientData,x => x.Client.Meta, true);

                    })
                    .Position(Position.Absolute)
                    .Width(0.3f)
                    .Height(0.1f)
                    .Right(0)
                    .Top(0);
                
                //Hotbar
                z1.Hstack(h2 =>
                    {
                        for (byte i = 0; i < 7; i++)
                        {
                            h2.InventorySlot(ClientData.Inventory, i, hotBarMode:true, clickAction: (e, slotId) =>
                            {
                                if (e.Mouse.LeftButton == ButtonState.Pressed)
                                    ClientData.Inventory.ActiveSlot = slotId;
                            });
                        }
                    })
                    .Position(Position.Absolute)
                    .Height(0.1f)
                    .Width(0.4f)
                    .Bottom(0);
            });

        }
    }
}