
using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UITextComponent<TModel> : UIComponent
    {
        #region Properties
        
        private readonly string _text;
        private readonly TModel _model;
        private readonly Func<TModel, string> _propertySelector;
        
        #endregion
        
        #region Constuctors
        
        public UITextComponent(TModel model, Func<TModel,string> propertySelector) : base(nameof(UITextComponent<TModel>))
        {
            _model = model;
            _propertySelector = propertySelector;
        }
        
        
        public UITextComponent(string text) : base(nameof(UITextComponent<TModel>))
        {
            _text = text;
            _propertySelector = null;
        }
        
        #endregion

        #region Render
        
        public override void Render(SpriteBatch graphics, Texture2D texture, SpriteFont font)
        {
            base.Render(graphics, texture, font);

            string text = _propertySelector == null ? WrapText(font,_text, RenderAttributes.Width,RenderAttributes.Height) : WrapText(font, _propertySelector(_model), RenderAttributes.Width, RenderAttributes.Height);
            
            DrawString(graphics, font, text, new Rectangle(RenderAttributes.X,RenderAttributes.Y,RenderAttributes.Width ,RenderAttributes.Height ));
        }
        
        #endregion
        
        //TODO make into helper
        private void DrawString(SpriteBatch graphics, SpriteFont font, string strToDraw, Rectangle boundaries)
        {
            
            (float x, float y) = font.MeasureString(strToDraw);
            
            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = System.Math.Min(boundaries.Width / x, boundaries.Height / y);

            scale *= (8f / 10f);//Applies a padding to the scale
            
            Vector2 position = new Vector2()
            {
                X = boundaries.X  - (int)(x * scale / 2) + (boundaries.Width) / 2,
                Y = boundaries.Y - (int)(y * scale / 2) + boundaries.Height / 2
            };

            // Draw the string to the sprite batch!
            graphics.DrawString(font, strToDraw, position, RenderAttributes.Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        
        #region TextWrapping
        
        public static string WrapText(SpriteFont font, string text, int maxLineWidth, int maxLineHeight)
        {
            //TODO do something if too tall in the future
            
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (sb.ToString() == "")
                        {
                            sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth, maxLineHeight));
                        }
                        else
                        {
                            sb.Append("\n" + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth, maxLineHeight));
                        }
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }

            return sb.ToString();
        }
        
        #endregion
    }
}