using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Graphics;
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
        
        #region Start
        
        public void Start(string key, Scene.Scene scene)
        {
            StopLayers();
            
            if (_allUILayers.ContainsKey(key))
            {
                _activeUILayers.Add(key, _allUILayers[key]);
                _allUILayers[key].Start(scene);

                BuildUILayer(key);
                
            }
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

        public UILayer GetLayer(string layerKey)
        {
            return _allUILayers.ContainsKey(layerKey) ? _allUILayers[layerKey] : null;
        }
        
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

//            if (node == null || string.IsNullOrEmpty(node.RenderAttributes.TextureKey))
//                return;
            
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