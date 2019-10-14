using System.Collections.Generic;
using BFB.Engine.Content;
using BFB.Engine.UI.Components;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI
{
    [UsedImplicitly]
    public class UIManager
    {
        #region Properties
        
        private readonly GraphicsDevice _graphicsDevice;
        private readonly BFBContentManager _contentManager;
        private readonly Dictionary<string,UILayer> _activeUILayers;
        private readonly Dictionary<string, UILayer> _allUILayers;
        
        #endregion
        
        #region Constructor
        
        public UIManager(GraphicsDevice graphicsDevice, BFBContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            
            _activeUILayers = new Dictionary<string, UILayer>();
            _allUILayers = new Dictionary<string, UILayer>();
        }
        
        #endregion
        
        #region AddUILayer(UILayer[] uiLayers)

        public void AddUILayer(IEnumerable<UILayer> uiLayers)
        {
            foreach (UILayer uiLayer in uiLayers)
            {
                AddUILayer(uiLayer);
            }
        }

        #endregion

        #region AddUILayer(UILayer uiLayer)

        public void AddUILayer(UILayer uiLayer)
        {
            if(!_allUILayers.ContainsKey(uiLayer.Key))
                _allUILayers.Add(uiLayer.Key, uiLayer);
        }

        #endregion
        
        public void Start(string key)
        {
            if (_allUILayers.ContainsKey(key) && !_activeUILayers.ContainsKey(key))
            {
                _activeUILayers.Add(key, _allUILayers[key]);
            }
        }
        
        #region Update

        public void Update()
        {
            foreach ((string _, UILayer uiLayer) in _activeUILayers)
            {
                uiLayer.SetRoot(new UIRootComponent(_graphicsDevice.Viewport.Bounds));
                uiLayer.Body();
                BuildComponent(uiLayer.RootUI);
            }
           
        }
        
        #endregion
        
        #region Draw

        public void Draw(SpriteBatch graphics)
        {
            foreach ((string _,UILayer uiLayer) in _activeUILayers)
            {
                RenderComponent(uiLayer.RootUI, graphics);
            }
        }
        
        #endregion
        
        #region BuildComponents

        /**
         * Builds a 
         */
        public void Build(string layer)
        {
            //TODO
        }
        
        /**
         * Recursively generates the UI structure and applies the UIConstraints and modifiers
         */
        public void BuildComponent(UIComponent node)
        {
            node.Build();

            foreach (UIComponent childNode in node.Children)
            {
                BuildComponent(childNode);
            }
        }

        #endregion

        #region RenderComponents
        
        /**
         * Recursive method to draw the UI
         */
        private void RenderComponent(UIComponent node, SpriteBatch graphics)
        {
            node.Render(graphics, _contentManager.GetTexture(node.TextureKey), _contentManager.GetFont(node.FontKey));
            
//            DrawBorder(new Rectangle(node.X,node.Y,node.Width,node.Height),1,Color.Black, graphics,_contentManager.GetTexture("default"));//For debug

            foreach (UIComponent childNode in node.Children)
            {
                RenderComponent(childNode, graphics);
            }
        }
        
        /**
         * Draws a border
         * TODO move to a drawing class with more drawing helpers. (Extension methods for drawing??)
         */
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch graphics, Texture2D texture)
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
        
        #region Dispose

         public void Dispose()
         {
             _activeUILayers.Clear();
             _allUILayers.Clear();
         }
         
         #endregion
    }
    
    
}