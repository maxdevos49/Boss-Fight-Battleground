using BFB.Engine.Content;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Helpers
{
    public static class DrawingExtensions
    {
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

        public static void DrawVector(this SpriteBatch graphics, Vector2 point, Vector2 vector, int thickness, Color color, BFBContentManager content)
        {
            Vector2 endPoint = new Vector2(point.X + vector.X, point.Y + vector.Y);

            graphics.DrawLine(point, endPoint,thickness,color,content);
        }

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

        public static void DrawAtlas(this SpriteBatch graphics, AtlasTexture atlasTexture, Rectangle rectangle, Color color)
        {
            graphics.Draw(atlasTexture.Texture,
                new Rectangle(rectangle.X,rectangle.Y,30,30),
                new Rectangle(atlasTexture.X,atlasTexture.Y,atlasTexture.Width,atlasTexture.Height),
                color);
        }
    }
}