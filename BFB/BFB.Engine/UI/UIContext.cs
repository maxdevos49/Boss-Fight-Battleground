using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI
{
    public class UIContext<TModel>
    {
        private readonly TModel _model;
        private IComponent _view;
        private readonly GraphicsDevice _graphicsDevice;
        
        public UIContext(GraphicsDevice graphicsDevice ,TModel model)
        {
            _graphicsDevice = graphicsDevice;
            _model = model;
            _view = InitView();
        }
        
        private IComponent InitView()
        {
            return new Component()
            {
                X = 0,
                Y = 0,
                Width = _graphicsDevice.Viewport.Width,
                Height = _graphicsDevice.Viewport.Height,
                Background = Color.Transparent,//Do not show this panel
                Color = Color.Black//Show text as black by default
            };
        }

        public IComponent GetRoot()
        {
            return _view;
        }

        public void Clear()
        {
            _view = InitView();
        }
        
    }
}