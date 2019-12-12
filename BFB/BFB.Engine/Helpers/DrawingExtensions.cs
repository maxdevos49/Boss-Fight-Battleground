using System;
using System.Text;
using BFB.Engine.Content;
using BFB.Engine.Math;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Helpers
{
    [Serializable]
    public enum ColorOption : byte
    {
        Black,
        White,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple
    }
    
    public static class DrawingExtensions
    {
        private static readonly Color[] ForegroundColors = 
        {
            Color.Black,
            Color.White, 
            Color.Red, 
            Color.Orange, 
            Color.Yellow, 
            Color.Green, 
            Color.Blue, 
            Color.Purple, 
        };

        private const float Transparency = 0.5f;
        private static readonly Color[] BackgroundColors = 
        {
            new Color(0,0,0,Transparency), 
            new Color(255,255,255,Transparency), 
            new Color(Color.Red.R,Color.Red.G,Color.Red.B,Transparency), 
            new Color(Color.Orange.R,Color.Orange.G,Color.Orange.B,Transparency), 
            new Color(Color.Yellow.R,Color.Yellow.G,Color.Yellow.B,Transparency), 
            new Color(Color.Green.R,Color.Green.G,Color.Green.B,Transparency), 
            new Color(Color.Blue.R,Color.Blue.G,Color.Blue.B,Transparency), 
            new Color(Color.Purple.R,Color.Purple.G,Color.Purple.B,Transparency)
        };
        
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
        
        public static void DrawAtlas(this SpriteBatch graphics, AtlasTexture atlasTexture, Rectangle dimensions, Color color, float rotation = 0, Vector2? origin = null)
        {
            graphics.Draw(atlasTexture.Texture,
                dimensions,
                new Rectangle(
                    atlasTexture.X,
                    atlasTexture.Y,
                    atlasTexture.Width-2,
                    atlasTexture.Height-2),
                color, 
                rotation,
                origin ?? Vector2.Zero,
                SpriteEffects.None,
                1);
        }
        
        #endregion
        
        #region DrawChatText

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="yOffset"></param>
        /// <param name="message"></param>
        /// <param name="container"></param>
        /// <param name="content"></param>
        /// <returns>The bounds of the area the message took</returns>
        public static int DrawChatText(this SpriteBatch graphics, int yOffset, ChatMessage message, UIComponent container, BFBContentManager content)
        {
            #region InitDimensions
            Rectangle bounds = new Rectangle
            {
                X = container.RenderAttributes.X,
                Y = container.RenderAttributes.Y + yOffset,
                Width = container.RenderAttributes.Width,
                Height = message.Height
            };

            message.Width = container.RenderAttributes.Width;
            #endregion
            
            SpriteFont font = content.GetFont(container.RenderAttributes.FontKey);
            Texture2D texture = content.GetTexture(container.RenderAttributes.TextureKey);
            
            #region Line Height and scale
            
            int lineHeight = (int)(content.GraphicsDevice.Viewport.Width / 25f * container.RenderAttributes.FontSize);
            float scale =  lineHeight / font.MeasureString(" ").Y;
            
            #endregion
            
            #region Draw Background
            
            graphics.Draw(texture, bounds, BackgroundColors[(int)message.BackgroundColor]);

            #endregion
            
            Vector2 cursor = new Vector2(bounds.X,bounds.Y);
            float startingHeight = cursor.Y;
            int spaceWidth = (int)(font.MeasureString(" ").X * scale);

            cursor = graphics.DrawColoredText(font, cursor, message.Header.Text, bounds, message.Header.ForegroundColor, scale);

            foreach (ChatText chatText in message.Body)
                cursor = graphics.DrawColoredText(font, cursor, chatText.Text, bounds, chatText.ForegroundColor, scale);

            message.Height = (int) (cursor.Y - startingHeight) + lineHeight;

            return (int) (cursor.Y - startingHeight) + lineHeight;
        }
        
        #endregion

        private static Vector2 DrawColoredText(this SpriteBatch graphics, SpriteFont font, Vector2 cursor, string text, Rectangle containerBounds, ColorOption color, float scale)
        {
            
            (float spaceWidth, float lineHeight) = font.MeasureString(" ") * scale;
            
            foreach (string word in text.Split(" "))
            {
                (float wordWidth, float _) = font.MeasureString(word) * scale;

                if (cursor.X + wordWidth > containerBounds.X + containerBounds.Width)
                {
                    cursor.X = containerBounds.X;
                    cursor.Y += lineHeight;
                }
                
                graphics.DrawString(
                    font, 
                    word,
                    cursor,
                    ForegroundColors[(int)color],
                    0, 
                    Vector2.Zero, 
                    scale,
                    SpriteEffects.None, 
                    0);

                cursor.X += wordWidth + spaceWidth;
            }

            return cursor;
        }
        
        #region DrawUIText

        public static void DrawUIText(this SpriteBatch graphics, UIComponent component, BFBContentManager content)
        {
            SpriteFont font = content.GetFont(component.RenderAttributes.FontKey);
            
            float scale = 1f;
            string text = component.Text;
            Vector2 position = new Vector2();
            (float iWidth, float iHeight) = font.MeasureString(text);
            
            #region Scale Text
            
            //Decide how to scale the text
            if (component.RenderAttributes.FontScaleMode == FontScaleMode.ContainerFitScale)
            {
                scale = System.Math.Min(component.RenderAttributes.Width / iWidth, component.RenderAttributes.Height / iHeight);
                scale *= 8f / 10f;
            }
            else if(component.RenderAttributes.FontScaleMode == FontScaleMode.FontSizeScale)
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