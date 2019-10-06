using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IComponent = BFB.Engine.UI.Components.IComponent;

namespace BFB.Engine.UI
{
    [UsedImplicitly]
    public class UIManager
    {

        /**
         * Recursive method to draw the UI
         */
        public static void RenderUI(IComponent uiStructure, SpriteBatch graphics, Texture2D texture, int num)
        {
//            Console.WriteLine($"Drawing {uiStructure.Name}");
            graphics.Draw(texture, new Rectangle(uiStructure.X, uiStructure.Y, uiStructure.Width, uiStructure.Height), uiStructure.Background);

            foreach (IComponent component in uiStructure.Children)
            {
                RenderUI(component, graphics, texture, num+1);
            }
        }

//        [UsedImplicitly]
//        public static IComponent BuildView(Action viewHandler)
//        {
//            return BuildView(new object(), viewHandler);//TODO
//        }
//        
//        [UsedImplicitly]
//        public static IComponent BuildView<TModel>(TModel model, Action viewHandler)
//        {
//            var context = BuildContext(model);
//            
//            return null;//TODO
//        }
//        
//        private static object BuildContext<TModel>(TModel model)
//        {
//            return null;//TODO
//        }
        
    }
    
    
}