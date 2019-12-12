using System;
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
        
        #region Start
        
        public void StartLayer(string key, Scene.Scene scene)
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
        
        #region LaunchLayer

        public void LaunchLayer(string key, Scene.Scene scene)
        {
            if (_allUILayers.ContainsKey(key) && !_activeUILayers.ContainsKey(key))
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

        #region GetLayer
        
        public UILayer GetLayer(string layerKey)
        {
            return _allUILayers.ContainsKey(layerKey) ? _allUILayers[layerKey] : null;
        }
        
        #endregion

        #region Update
        
        public void UpdateLayers(GameTime time)
        {
            foreach ((string _, UILayer uiLayer) in _activeUILayers.ToList())
                uiLayer.UpdateLayer(time);
        }
        
        #endregion
        
        #region Draw

        public void Draw(SpriteBatch graphics)
        {
            foreach ((string _, UILayer uiLayer) in _activeUILayers.ToList())
            {
                RenderComponents(uiLayer.RootUI, graphics, uiLayer);
                uiLayer.Draw(graphics, _contentManager);    
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
            if (!_activeUILayers.ContainsKey(layer)) 
                return;
            
            _activeUILayers[layer].InitializeRoot(new UIRootComponent(_graphicsDevice.Viewport.Bounds));
            _activeUILayers[layer].Body();
            BuildComponent(_activeUILayers[layer], _activeUILayers[layer].RootUI);
        }
        
        #endregion
        
        #region BuildComponents

        /**
         * Recursively generates the UI structure and applies the UIConstraints and modifiers
         */
        public void BuildComponent(UILayer layer, UIComponent  node)
        {
            
            node.ParentLayer = layer;
            node.Build(layer);

            foreach (UIComponent childNode in node.Children)
                BuildComponent(layer, childNode);
        }

        #endregion

        #region RenderComponents
        
        /**
         * Recursive method to draw the UI
         */
        private void RenderComponents(UIComponent node, SpriteBatch graphics, UILayer layer)
        {

            if (node == null || node.RenderAttributes.Width < 10 || node.RenderAttributes.Height < 10)
                return;
            
            if (node.RenderAttributes.Overflow == Overflow.Hide)
            {
                #region graphics.Begin()
                //Stop global buffer
                graphics.End();
            
                //indicate how we are redrawing the text
                RasterizerState r = new RasterizerState {ScissorTestEnable = true};
                graphics.GraphicsDevice.ScissorRectangle = new Rectangle(node.RenderAttributes.X,node.RenderAttributes.Y,node.RenderAttributes.Width,node.RenderAttributes.Height);
            
                //Start new special buffer
                graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,r);
                #endregion
            }
            
            node.Render(graphics, _contentManager);

            #region Debug Border
            if (layer.Debug)
                    graphics.DrawBorder(
                        new Rectangle(
                            node.RenderAttributes.X,
                            node.RenderAttributes.Y,
                            node.RenderAttributes.Width,
                            node.RenderAttributes.Height),
                        1,
                        Color.Black,
                        _contentManager.GetTexture("default"));
            #endregion

            #region Focus Border
            
            if (node.Focused)
                graphics.DrawBorder(
                    new Rectangle(
                        node.RenderAttributes.X,
                        node.RenderAttributes.Y,
                        node.RenderAttributes.Width,
                        node.RenderAttributes.Height),
                    3,
                    Color.Red,
                    _contentManager.GetTexture("default"));
            
            #endregion

            foreach (UIComponent childNode in node.Children)
                RenderComponents(childNode, graphics, layer);
            
            if (node.RenderAttributes.Overflow == Overflow.Hide)
            {
                #region graphics.End()
                graphics.End();
                graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                #endregion
            }
        }


        #endregion

        #region ProcessEvents
     
         public bool ProcessEvents(InputEvent inputEvent)
         {
             //gets all possible events based on input event
             List<UIEvent> uiEvents = UIEvent.ConvertInputEventToUIEvent(inputEvent);

             //Get layers to loop through in reverse so we start with top layer and work down for events
             List<KeyValuePair<string, UILayer>> activeLayers = _activeUILayers.ToList();
             activeLayers.Reverse();
             
             //Passes a fresh list to each layer
             foreach ((string _, UILayer uiLayer) in activeLayers)
             {
                 //if event was caught then we stop here
                 if (uiLayer.ProcessEvents(uiEvents))
                     return false;
                 
                 //see if uiLayer can use the event
                 if(uiLayer.ProcessInputEvent(inputEvent))
                     return false;
                 
                 //If the uiLayer should block the continuation of the event propagation
                 if (uiLayer.BlockInput)
                     return false;
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