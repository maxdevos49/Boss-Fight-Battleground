using System.Text;
using BFB.Engine.Content;
using BFB.Engine.Math;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Helpers
{
    public static class DrawingExtensions
    {
        #region DrawBorder
        
        public static void DrawBorder(this SpriteBatch graphics, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, Texture2D texture)
        {
            // Draw top line
            graphics.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            
            // Draw left line
            graphics.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            graphics.Draw(texture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            
            // Draw bottom line
            graphics.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }
        
        #endregion

        #region DrawBackedText
        
        public static void DrawBackedText(this SpriteBatch graphics, string text, BfbVector position, BFBContentManager content, float scale = 1f)
        {
            SpriteFont font = content.GetFont("default");
            Texture2D texture = content.GetTexture("default");
            
            (float width, float height) = font.MeasureString(text);

            width *= scale;
            height *= scale;
            
            //Background
            graphics.Draw(
                    texture,
                    new Rectangle((int)position.X - 2, (int)position.Y - 2, (int)width + 4,(int)height + 2),
                    new Color(0,0,0,0.5f));
            
            graphics.DrawString(
                            font,
                            text, 
                            position.ToVector2(),
                            Color.White,
                            0f, 
                            Vector2.Zero, 
                            scale,
                            SpriteEffects.None,
                            1);
        }
        
        #endregion

        #region DrawVector
        
        public static void DrawVector(this SpriteBatch graphics, Vector2 point, Vector2 vector, int thickness, Color color, BFBContentManager content)
        {
            Vector2 endPoint = new Vector2(point.X + vector.X, point.Y + vector.Y);

            graphics.DrawLine(point, endPoint,thickness,color,content);
        }

        #endregion
        
        #region DrawLine
        
        public static void DrawLine(this SpriteBatch graphics, Vector2 p1, Vector2 p2, int thickness, Color color, BFBContentManager content)
        {
            Vector2 edge = p2 - p1;//gets slope
            int length = (int)System.Math.Sqrt(System.Math.Pow(p2.X - p1.X, 2) + System.Math.Pow(p2.Y - p1.Y, 2));
            float angle = (float)System.Math.Atan2(edge.Y , edge.X);
            
            graphics.Draw(
                    content.GetTexture("default"), 
                    new Rectangle((int)p1.X,(int) p1.Y, length, thickness), 
                    null,
                    color,
                    angle,
                    Vector2.Zero,
                    SpriteEffects.None,
                    1 );

        }
        
        #endregion

        #region DrawAtlas
        
        public static void DrawAtlas(this SpriteBatch graphics, AtlasTexture atlasTexture, Rectangle rectangle, Color color, float scale = 1f)
        {
            graphics.Draw(atlasTexture.Texture,
                new Rectangle(rectangle.X,rectangle.Y,(int)(rectangle.Width * scale),(int)(rectangle.Width * scale)),
                new Rectangle(atlasTexture.X,atlasTexture.Y,atlasTexture.Width-2,atlasTexture.Height-2),
                color);
        }
        
        #endregion
        
        #region DrawText

        public static void DrawUIText(this SpriteBatch graphics, UIComponent component, BFBContentManager content)
        {
            SpriteFont font = content.GetFont(component.RenderAttributes.FontKey);
            
            float scale = 1f;
            string text = component.Text;
            Vector2 position = new Vector2();
            (float iWidth, float iHeight) = font.MeasureString(text);
            
            #region Scale Text
            
            //Decide how to scale the text
            if (component.RenderAttributes.TextScaleMode == TextScaleMode.ContainerFitScale)
            {
                scale = System.Math.Min(component.RenderAttributes.Width / iWidth, component.RenderAttributes.Height / iHeight);
                scale *= 8f / 10f;
            }
            else if(component.RenderAttributes.TextScaleMode == TextScaleMode.FontSizeScale)
            {
                int pixelHeight = (int)(content.GraphicsDevice.Viewport.Width / 25f * component.RenderAttributes.FontSize);
                scale =  pixelHeight / iHeight;
            }
            
            #endregion

            #region Wrap Text
            
            if (component.RenderAttributes.TextWrap == TextWrap.Wrap)
            {
                text =  WrapUIComponentText(font, component.Text, component, scale);
            }
            
            #endregion

            #region Justify text
            if (component.RenderAttributes.JustifyText == JustifyText.Center)
                position.X = component.RenderAttributes.X - (int) (iWidth * scale / 2) + component.RenderAttributes.Width / 2;
            else if (component.RenderAttributes.JustifyText == JustifyText.End)
                position.X = component.RenderAttributes.X + component.RenderAttributes.Width -  iWidth * scale;
            else if (component.RenderAttributes.JustifyText == JustifyText.Start)
                position.X = component.RenderAttributes.X;
            #endregion
            
            #region Vertical Align Text
            
            //Vertical align text
            if (component.RenderAttributes.VerticalAlignText == VerticalAlignText.Center)
                position.Y = component.RenderAttributes.Y - (int) (iHeight * scale / 2) + component.RenderAttributes.Height / 2;
            else if (component.RenderAttributes.VerticalAlignText == VerticalAlignText.End)
                position.Y = component.RenderAttributes.Y + component.RenderAttributes.Height -  iHeight * scale;
            else if (component.RenderAttributes.VerticalAlignText == VerticalAlignText.Start)
                position.Y = component.RenderAttributes.Y;
            
            #endregion
            
            graphics.DrawString(
                font, 
                text,
                position,
                component.RenderAttributes.Color,
                0, 
                Vector2.Zero, 
                scale,
                SpriteEffects.None, 
                0);
        }
        
        #endregion
        
        #region Text Wrap
        
        private static string WrapUIComponentText(SpriteFont font, string text, UIComponent component, float scale)
        {
            string[] words = component.Text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            (float spaceWidth, float _) = font.MeasureString(" ") * scale;

            
            foreach (string word in words)
            {
                (float wordWidth,float _) = font.MeasureString(word) * scale;

                if (lineWidth + wordWidth < component.RenderAttributes.Width)
                {
                    sb.Append(word + " ");
                    lineWidth += wordWidth + spaceWidth;
                }
                else
                {
                    if (wordWidth > component.RenderAttributes.Width)
                    {
                        if (sb.ToString() == "")
                            sb.Append(WrapUIComponentText(font, word.Insert(word.Length / 2, " ") + " ", component, scale));
                        else
                            sb.Append("\n" + WrapUIComponentText(font, word.Insert(word.Length / 2, " ") + " ", component, scale));
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = wordWidth + spaceWidth;
                    }
                }
            }


            return sb.ToString();
        }
        
        #endregion
    }
}