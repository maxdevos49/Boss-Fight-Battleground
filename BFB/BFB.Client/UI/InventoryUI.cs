using System;
using System.Linq.Expressions;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Inventory;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class InventoryUI : UILayer
    {
        
        private ClientInventory Inventory { get; set; }
        private BfbVector Mouse { get; set; }
        
        public InventoryUI() : base(nameof(InventoryUI))
        {
            BlockInput = true;
            Mouse = new BfbVector();
        }
        
        #region Init
        
        protected override void Init()
        {
            
            Inventory = ClientDataRegistry.GetInstance().Inventory;

         
            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                    case Keys.E:
                        UIManager.StopLayer(Key);
                        break;
                }
                e.StopPropagation();
            });
            
            AddInputListener("mousemove", e =>
            {
                Mouse = new BfbVector(e.Mouse.X, e.Mouse.Y);
            });
        }
        
        #endregion
        
        #region Draw

        public override void Draw(SpriteBatch graphics, BFBContentManager content)
        {
            InventorySlot slot = ClientDataRegistry.GetInstance()?.Client?.Meta?.MouseSlot;

            if (slot == null)
                return;
            
            graphics.DrawAtlas(
                content.GetAtlasTexture(slot.TextureKey),
                new Rectangle(
                    (int)Mouse.X - 15, 
                    (int)Mouse.Y - 15, 
                    30, 
                    30), 
                Color.White);
            
        }

        #endregion
        
        #region Body
        
        public override void Body()
        {
            
            RootUI.Background(Color.Transparent);
            
            RootUI.Hstack(h1 =>
                {
                    h1.Spacer();

                    h1.Vstack(v2 =>
                        {

                            for (int i = 3; i >= 0; i--)
                            {
                                if (i == 0)
                                    v2.Spacer(2);
                                
                                int i2 = i;
                                v2.Hstack(h2 =>
                                {
                                    for (int j = 0; j < 7; j++)
                                    {
                                        int i1 = i2;
                                        int j1 = j;
                                        
                                        h2.InventorySlot(Inventory, (byte)(i1 * 7 + j1), clickAction: (e, slotId) =>
                                        {
                                            ParentScene.Client.Emit("/inventory/select", new InventoryActionMessage
                                            {
                                                LeftClick = e.Mouse.LeftButton == ButtonState.Pressed,
                                                RightClick = e.Mouse.RightButton == ButtonState.Pressed,
                                                SlotId = slotId
                                            });
                                        });
                                    }
                                })
                                    .Grow(10);
                                
                            }
                            
                        })
                        .AspectRatio(1.78f)
                        .Center()
                        .Grow(4);
                    
                    h1.Spacer(); 
                })
                .Background(new Color(0,0,0,0.4f));

        }
        
        #endregion
    }
}