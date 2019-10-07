using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string,UILayer> _ActiveUILayers;
        private readonly Dictionary<string, UILayer> _AllUILayers;
        private readonly Dictionary<string, Texture2D> _UITextures;
        
        #endregion
        
        #region Constructor
        
        public UIManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            
            _ActiveUILayers = new Dictionary<string, UILayer>();
            _AllUILayers = new Dictionary<string, UILayer>();
            _UITextures = new Dictionary<string, Texture2D>();
            
            //Init default panel texture
            Texture2D rectangleTexture = new Texture2D(_graphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });
            
            _UITextures.Add("default", rectangleTexture);
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
            if(!_AllUILayers.ContainsKey(uiLayer.Key))
                _AllUILayers.Add(uiLayer.Key, uiLayer);
        }

        #endregion

        public void Start(string key)
        {
            if (_AllUILayers.ContainsKey(key) && !_ActiveUILayers.ContainsKey(key))
            {
                _ActiveUILayers.Add(key, _AllUILayers[key]);
            }
        }
        
        #region Update

        public void Update()
        {
            foreach ((string key, UILayer uiLayer) in _ActiveUILayers)
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
            foreach ((string key,UILayer uiLayer) in _ActiveUILayers)
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
            Console.WriteLine($"{node.Name}");
            node.Render(graphics, _UITextures[node.TextureKey]);

            foreach (UIComponent childNode in node.Children)
            {
                RenderComponent(childNode, graphics);
            }
        }

     #endregion
        
        #region Dispose

         public void Dispose()
         {
             foreach ((string key,Texture2D texture) in _UITextures)
             {
                 texture.Dispose();
             }
             
             _UITextures.Clear();
             _ActiveUILayers.Clear();
             _AllUILayers.Clear();
         }
         
         #endregion
    }
    
    
}