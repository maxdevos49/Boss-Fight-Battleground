using Microsoft.Xna.Framework;

namespace BFB.Engine.UI
{
    public class UIAttributes
    {
        #region Properties
        
        public string TextureKey { get; set; }
        
        public string FontKey { get; set; }
        
        public float FontSize { get; set; }
        
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int OffsetX { get; set; }
        
        public int OffsetY { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }

        public Color Color { get; set; }
        
        public Color Background { get; set; }
        
        public Color BorderColor { get; set; }
     
        public int Grow { get; set; }
        
        public int BorderSize { get; set; }
        
        public Position Position { get; set; }
        
        public Sizing Sizing { get; set; }
        
        public Overflow Overflow { get; set; }
        
        public TextWrap TextWrap { get; set; }
        
        public JustifyText JustifyText { get; set; }
        
        public VerticalAlignText VerticalAlignText { get; set; }
        
        public FontScaleMode FontScaleMode { get; set; }
        
        public StackDirection StackDirection { get; set; }
        
        #endregion

        #region Constructor

        public UIAttributes()
        {
            Background = Color.Transparent;
            Color = Color.Black;
            BorderColor = Color.Black;
            FontSize = 1;
            Grow = 1;
            Position = Position.Relative;
            Sizing = Sizing.Proportion;
            Overflow = Overflow.Show;
            StackDirection = StackDirection.None;
            TextWrap = TextWrap.Wrap;
            JustifyText = JustifyText.Center;
            VerticalAlignText = VerticalAlignText.Center;
            FontScaleMode = FontScaleMode.FontSizeScale;
        }
        
        #endregion
        
        /**
         * This methods takes a attribute object and overrides styles that
         * are present and not null. All properties are null by default and
         * will only be cascaded if is present
         */
        public UIAttributes CascadeAttributes(UIAttributes cascadingAttributes)
        {
            return new UIAttributes
            {
                TextureKey = cascadingAttributes.TextureKey ?? TextureKey,
                FontKey = cascadingAttributes.FontKey ?? FontKey,
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                FontSize = cascadingAttributes.FontSize != 1f ? cascadingAttributes.FontSize : FontSize,
                X = cascadingAttributes.X != 0 ? cascadingAttributes.X : X,
                Y = cascadingAttributes.Y != 0 ? cascadingAttributes.Y : Y,
                OffsetX = cascadingAttributes.OffsetX != 0 ? cascadingAttributes.OffsetX : OffsetX,
                OffsetY = cascadingAttributes.OffsetY != 0 ? cascadingAttributes.OffsetY : OffsetY,
                Width = cascadingAttributes.Width != 0 ? cascadingAttributes.Width : Width,
                Height = cascadingAttributes.Height != 0 ? cascadingAttributes.Height : Height,
                Color = cascadingAttributes.Color != Color.Black ? cascadingAttributes.Color : Color,
                Background = cascadingAttributes.Background != Color.Transparent ? cascadingAttributes.Background : Background,
                Grow = cascadingAttributes.Grow != 1 ? cascadingAttributes.Grow : Grow,
                BorderSize = cascadingAttributes.BorderSize != 0 ? cascadingAttributes.BorderSize : BorderSize,
                BorderColor = cascadingAttributes.BorderColor != Color.Black ? cascadingAttributes.BorderColor : BorderColor,
                Position = cascadingAttributes.Position != Position.Relative ? cascadingAttributes.Position : Position,
                Sizing = cascadingAttributes.Sizing != Sizing.Proportion ? cascadingAttributes.Sizing : Sizing,
                Overflow = cascadingAttributes.Overflow != Overflow.Show ? cascadingAttributes.Overflow : Overflow,
                JustifyText = cascadingAttributes.JustifyText != JustifyText.Center ? cascadingAttributes.JustifyText : JustifyText,
                VerticalAlignText = cascadingAttributes.VerticalAlignText != VerticalAlignText.Center ? cascadingAttributes.VerticalAlignText : VerticalAlignText,
                TextWrap = cascadingAttributes.TextWrap != TextWrap.Wrap ? cascadingAttributes.TextWrap : TextWrap,
                FontScaleMode = cascadingAttributes.FontScaleMode != FontScaleMode.FontSizeScale ? cascadingAttributes.FontScaleMode : FontScaleMode,
                StackDirection= cascadingAttributes.StackDirection != StackDirection.None ? cascadingAttributes.StackDirection : StackDirection,
            };
        }
        
    }
    
    public enum StackDirection : byte
    {
        Inherit,
        Vertical,
        Horizontal,
        None
    }

    public enum Position : byte
    {
        Inherit,
        Relative,
        Absolute
    }

    public enum Sizing : byte
    {
        Inherit,
        Proportion,
        Dimension        
    }

    public enum Overflow : byte
    {
        Inherit,
        Hide,
        Show,
    }

    public enum JustifyText : byte
    {
        Inherit,
        Start,
        Center,
        End
    }

    public enum VerticalAlignText : byte
    {
        Inherit,
        Start,
        Center,
        End
    }

    public enum TextWrap : byte
    {
        Inherit,
        NoWrap,
        Wrap
    }

    public enum FontScaleMode : byte
    {
        Inherit,
        ContainerFitScale,
        FontSizeScale
    }
}