using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Helpers;
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
        
        #region StartLayer
        
<<<<<<< HEAD
        public void StartLayer(string key)
=======
        public void Start(string key, Scene.Scene scene)
>>>>>>> 25eb9767d2ff3738c9e8f07fa547477d11c8cf6b
        {
            StopLayers();

            if (!_allUILayers.ContainsKey(key)) return;
            
<<<<<<< HEAD
            _activeUILayers.Add(key, _allUILayers[key]);
            _allUILayers[key].Start();
=======
            if (_allUILayers.ContainsKey(key))
            {
                _activeUILayers.Add(key, _allUILayers[key]);
                _allUILayers[key].Start(scene);
>>>>>>> 25eb9767d2ff3738c9e8f07fa547477d11c8cf6b

            BuildUILayer(key);
        }
        
        #endregion
        
        #region LaunchLayer

        public void LaunchLayer(string key)
        {
            if (!LayerExists(key) || ActiveLayerExists(key)) return;
            
            // Add to active layers
            _activeUILayers.Add(key, _allUILayers[key]);
            
            _activeUILayers[key].Start();
        }
        
        #endregion
        
        #region StopLayer

        public void StopLayer(string key)
        {
            if (!_activeUILayers.ContainsKey(key)) return;
            
            _activeUILayers[key].Stop();
            _activeUILayers.Remove(key);

        }
        
        #endregion
        
        #region StopLayers

        public void StopLayers()
        {
            foreach ((string _, UILayer layer) in _activeUILayers)
                layer.Stop();
            
            _activeUILayers.Clear();
        }
        
        #endregion

        #region GetLayer
        public UILayer GetLayer(string layerKey)
        {
            return _allUILayers.ContainsKey(layerKey) ? _allUILayers[layerKey] : null;
        }
        
        #endregion
        
        #region ActiveLayerExists

        /// <summary>
        /// Checks if the layer is running or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public bool ActiveLayerExists(string key)
        {
            return _activeUILayers.Any(scene => key == scene.Key);
        }

        #endregion
        
        #region LayerExists
         
        /// <summary>
        /// Checks if the layer exist regardless if it is running
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public bool LayerExists(string key)
        {
            return _allUILayers.ContainsKey(key);
        }
         
        #endregion

        #region Draw

        public void Draw(SpriteBatch graphics)
        {
            foreach ((string _,UILayer uiLayer) in _activeUILayers.ToList())
            {
                RenderComponents(uiLayer.RootUI, graphics, uiLayer);
            }
        }
        
        #endregion
        
        #region Window Resize

        public void WindowResize()
        {
            
            foreach ((string key, UILayer _) in _activeUILayers)
                BuildUILayer(key);
        }
        
        #endregion
        
        #region Build UI Layer

        private void BuildUILayer(string layer)
        {
                _activeUILayers[layer].InitializeRoot(new UIRootComponent(_graphicsDevice.Viewport.Bounds));
                _activeUILayers[layer].Body();
                BuildComponent(_activeUILayers[layer], _activeUILayers[layer].RootUI);
        }
        
        #endregion
        
        #region BuildComponents

        /**
         * Recursively generates the UI structure and applies the UIConstraints and modifiers
         */
        private void BuildComponent(UILayer layer, UIComponent  node)
        {
            node.Build(layer);

            foreach (UIComponent childNode in node.Children)
            {
                BuildComponent(layer, childNode);
            }
        }

        #endregion

        #region RenderComponents
        
        /**
         * Recursive method to draw the UI
         */
        private void RenderComponents(UIComponent node, SpriteBatch graphics, UILayer layer)
        {
            
            node.Render(graphics, _contentManager.GetTexture(node.RenderAttributes.TextureKey), _contentManager.GetFont(node.RenderAttributes.FontKey));
            
            if(layer.Debug)
                graphics.DrawBorder(new Rectangle(node.RenderAttributes.X,node.RenderAttributes.Y,node.RenderAttributes.Width,node.RenderAttributes.Height),1,Color.Black,_contentManager.GetTexture("default"));//For debug
            
            if(node.Focused)
                graphics.DrawBorder(new Rectangle(node.RenderAttributes.X,node.RenderAttributes.Y,node.RenderAttributes.Width,node.RenderAttributes.Height),3,Color.Red,_contentManager.GetTexture("default"));//For Focus

            foreach (UIComponent childNode in node.Children)
            {
                RenderComponents(childNode, graphics, layer);
            }

        }


        #endregion

        #region ProcessEvents
     
         public bool ProcessEvents(InputEvent inputEvent)
         {
             //gets all possible events based on input event
             List<UIEvent> uiEvent = UIEvent.ConvertInputEventToUIEvent(inputEvent);
             
             //Passes a fresh list to each layer
             // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator. This suggestion does not compile
             foreach ((string _, UILayer uiLayer) in _activeUILayers.ToList())
             {
                 if (uiLayer.ProcessEvents(uiEvent))
                     return true;
             }
             
             return true;
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