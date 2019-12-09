using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Graphics.GraphicsComponents
{
    public class AnimationComponent : IGraphicsComponent
    {
        private const int GameFps = 60;

        private readonly AnimatedTexture _animatedTexture;
        private AnimationSet _currentAnimationSet;
        private AnimationState _previousState;
        private Rectangle _frameSelector;
        private int _currentFrame;
        private int _frameTicksLeft;

        public AnimationComponent(AnimatedTexture animatedTexture)
        {
            _animatedTexture = animatedTexture;
            _previousState = AnimationState.None;
            _currentAnimationSet = _animatedTexture.AnimationSets[AnimationState.IdleRight];
            _frameSelector.Width = _animatedTexture.FrameWidth;
            _frameSelector.Height = _animatedTexture.FrameHeight;
            
            _frameTicksLeft = 0;
        }
        

        public void Update(ClientEntity entity)
        {
            //Always tick animation
            _frameTicksLeft -= 1;
            
            //Check for frame state change from server
            if (entity.AnimationState != _previousState)
            {
                _frameTicksLeft = 0;
                _currentFrame = _animatedTexture.AnimationSets[entity.AnimationState].FrameStart;
            }

            //Check if frame change is ready
            if (_frameTicksLeft > 0) 
                return;
            
            //If we reach this line then its time for the next frame
            _previousState = entity.AnimationState;
            
            _currentAnimationSet = _animatedTexture.AnimationSets[entity.AnimationState];
            
            _frameTicksLeft = GameFps/_currentAnimationSet.Fps;

            //increment frame number
            _currentFrame += 1;
            
            //check frame number bounds
            if (_currentFrame >= _currentAnimationSet.FrameEnd)
                _currentFrame = _currentAnimationSet.FrameStart;
            
            //Calculate the frame row and column to select
            int currentRow = _currentFrame /_animatedTexture.Columns;
            int currentCol = _currentFrame % _animatedTexture.Columns;

            //Move rectangle select box to correct frame
            _frameSelector.X = _frameSelector.Width * currentCol;
            _frameSelector.Y = _frameSelector.Height * currentRow;
        }

        public void Draw(ClientEntity entity, SpriteBatch graphics, BFBContentManager contentManager, float worldScale)
        {
            
            graphics.Draw(_animatedTexture.Texture,
                entity.Position.ToVector2(),
                _frameSelector,
                _animatedTexture.ParsedColor,
                entity.Rotation,
                entity.Origin.ToVector2() - (entity.Dimensions.ToVector2()/2),
                 _animatedTexture.Scale * worldScale,
                !_currentAnimationSet.Mirror ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1);

            if (entity.Meta?.Holding?.AtlasKey == null)
                return;

            int x = entity.Facing == DirectionFacing.Left ? entity.Left : entity.Right - 15;
            int y = (int)(entity.Height / 2f + entity.Position.Y);

            var atlas = contentManager.GetAtlasTexture(entity.Meta.Holding.AtlasKey);
            graphics.DrawAtlas(atlas, new Rectangle(x-atlas.Width/2, y-atlas.Height*2, atlas.Width*2,atlas.Height*2), Color.White);
            
            
            if (entity.Meta.Holding.ItemType == ItemType.Wall)
                graphics.Draw(contentManager.GetTexture("default"),
                    new Rectangle(x,y,(int)(30 * 0.6f),(int)(30* 0.6f)),
                    new Rectangle(0,0,1,1),
                    new Color(0,0,0,0.6f));
            
        }
    }
}
