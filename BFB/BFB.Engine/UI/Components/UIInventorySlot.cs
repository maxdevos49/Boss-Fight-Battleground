using System;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Helpers;
using BFB.Engine.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIInventorySlot : UIComponent
    {
        private const int Padding = 10;
        private bool _hover;
        private readonly byte _slotId;
        private readonly bool _hotBarMode;
        private readonly ClientInventory _inventory;

        public UIInventorySlot(ClientInventory inventory, byte slotId, Action<UIEvent,byte> clickAction = null, Action<UIEvent,byte> enterAction = null, bool hotBarMode = false) : base(nameof(UIInventorySlot))
        {
            _inventory = inventory;
            _slotId = slotId;
            _hover = false;
            _hotBarMode = hotBarMode;

            Color color = _hotBarMode ? Color.Black : Color.Silver;

            this.AspectRatio(1)
                .Background(new Color(0,0,0,0.5f))
                .Border(3, color)
                .FontSize(0.5f);
            
            AddEvent("click", e =>
            {
                clickAction?.Invoke(e, slotId);
            });
            AddEvent("hover", e =>
            {
                RenderAttributes = DefaultAttributes.CascadeAttributes(new UIAttributes
                {
                    Background = new Color(0,0,0,0.2f)
                });
            });
            AddEvent("mouseenter", e =>
            {
                enterAction?.Invoke(e,_slotId);
                _hover = true;
            });
            AddEvent("mouseleave", e => _hover = false);
        }
        
        #region Render
        
        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            base.Render(graphics, content);
            
            #region DrawHotbarBorder
            
            if (_slotId == _inventory.ActiveSlot && _hotBarMode)
            {
                graphics.DrawBorder(
                    new Rectangle(
                        RenderAttributes.X - 2,
                        RenderAttributes.Y - 2,
                        RenderAttributes.Width + 2*2,
                        RenderAttributes.Height + 2*2),
                    5, 
                    Color.Silver,
                    content.GetTexture("default"));
            }
            
            #endregion
            
            #region Draw Item
            
            if (_inventory.GetSlots().ContainsKey(_slotId))
            {
                InventorySlot slot = _inventory.GetSlots()[_slotId];

                if (string.IsNullOrEmpty(slot.TextureKey))
                    return;
                
                AtlasTexture atlas = content.GetAtlasTexture(slot.TextureKey);

                int padding = Padding;
                
                if (_hover)
                    padding -= 3;
                
                int maxHeight = RenderAttributes.Height - padding * 2;
                int scale = maxHeight / atlas.Height;
                
                int width = atlas.Width * scale;
                int height = atlas.Height * scale;

                int x = RenderAttributes.X + padding;
                int y = RenderAttributes.Y + padding;
            
                graphics.DrawAtlas(atlas, new Rectangle(x, y, width, height), Color.White);
                
                if(slot.ItemType == ItemType.Wall)
                    graphics.DrawAtlas(atlas, new Rectangle(x, y, width, height), new Color(0,0,0,0.4f));

                //draw count
                if (slot.Count <= 1) 
                    return;
                
                #region Draw Stack Count
                
                SpriteFont font = content.GetFont(RenderAttributes.FontKey);
                graphics.DrawString(
                    font,
                    slot.Count.ToString(),
                    new Vector2(
                        RenderAttributes.X + padding,
                        RenderAttributes.Y + padding ),
                    Color.White,
                    0,
                    Vector2.Zero,
                    RenderAttributes.FontSize * (graphics.GraphicsDevice.Viewport.Width/25f)/font.MeasureString(" ").Y,
                    SpriteEffects.None,
                    1);
                
                #endregion
            }
            
            #endregion
        }
        
        #endregion
    }
}