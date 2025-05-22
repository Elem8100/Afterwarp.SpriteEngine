using System.Numerics;
namespace Afterwarp.SpriteEngine;

public static class UInt
{
    public static uint ARGB(int Alpha, int Red, int Green, int Blue)
    {
        return (uint)Red | (((uint)Green) << 8) | (((uint)Blue) << 16) | (((uint)Alpha) << 24);
    }

    public static uint HSLColor(int Hue, int Saturation, int Lightness)
    {
        //Normal =0,0,0
        //Hue Range:        -180..+180
        //Saturation Range: -100..+100
        //Lightness  Range: -100..+100
        return ARGB(255, 128 + (int)(Lightness * 1.275f), 128 + (int)(Saturation * 1.275f), -128 + (int)(Hue * -0.708f));
    }
}

public enum FilterLevel
{
    None,
    Nearest,
    Linear,
    Linear_Cubic
}

public enum CanvasAttr
{
    None,
    ColorAdjust,
    ColorAdjust_Cubic
}

public class GameCanvas
{
    public static void SetFilterLevel(FilterLevel FilterLevel)
    {
        switch (FilterLevel)
        {
            case FilterLevel.None:
                Game.Canvas.SamplerState = new CanvasSamplerState(TextureFilter.None, TextureFilter.None);
                break;
            case FilterLevel.Nearest:
                Game.Canvas.SamplerState = new CanvasSamplerState(TextureFilter.Nearest, TextureFilter.Nearest);
                break;

            case FilterLevel.Linear:
                Game.Canvas.SamplerState = new CanvasSamplerState(TextureFilter.Linear, TextureFilter.Linear);
                break;

            case FilterLevel.Linear_Cubic:
                Game.Canvas.SamplerState = new CanvasSamplerState(TextureFilter.Linear, TextureFilter.Linear);
                Game.Canvas.Attributes = Canvas.Attribute.Cubic;
                break;
        }
    }

    public static void SetCanvasAttr(CanvasAttr CanvasAttr)
    { 
        switch (CanvasAttr)
        {
            case CanvasAttr.None:
                Game.Canvas.Attributes = 0;
                break;
            case CanvasAttr.ColorAdjust:
                Game.Canvas.Attributes = Canvas.Attribute.ColorAdjust;
                break;
            case CanvasAttr.ColorAdjust_Cubic:
                Game.Canvas.Attributes = Canvas.Attribute.ColorAdjust | Canvas.Attribute.Cubic;
                break;
        }
    }

    public static void Draw(Texture Texture, float X, float Y, BlendingEffect BlendingEffect = BlendingEffect.Normal)
    {
        TextureParameters Parameters = Texture.Parameters;
        Game.Canvas.Attributes = 0;
        Game.Canvas.Quad(Texture, new Quad(X, Y, Parameters.Width, Parameters.Height), null, null, BlendingEffect);
    }

    public static void DrawColor1(Texture Texture, float X, float Y, uint Color,
        bool FlipX = false, bool FlipY = false, BlendingEffect BlendingEffect = BlendingEffect.Normal)
    {
        TextureParameters Parameters = Texture.Parameters;
        Game.Canvas.Attributes = 0;
        Quad DestQuad = new Quad(X, Y, Parameters.Width, Parameters.Height);
        Quad SourceQuad = Quad.Unity;
        if (FlipX)
            SourceQuad = SourceQuad.Mirror;
        if (FlipY)
            SourceQuad = SourceQuad.Flip;
        Game.Canvas.Quad(Texture, DestQuad, new ColorRect(Color), SourceQuad, BlendingEffect);
    }

    public static void DrawCrop(Texture Texture, float X, float Y, Rect CropRect,
      bool FlipX = false, bool FlipY = false, float Angle = 0, bool DoCenter = false, float ScaleX = 1, float ScaleY = 1,
      uint Color = 0xFFFFFFFFu, int Hue = 0, int Saturation = 0, int Lightness = 0,
      BlendingEffect BlendingEffect = BlendingEffect.Normal, FilterLevel FilterLevel = FilterLevel.None)
    { 
        TextureParameters Parameters = Texture.Parameters;
     
        SetFilterLevel(FilterLevel);
        if (Hue != 0 || Saturation != 0 || Lightness != 0)
        {
            if (FilterLevel == FilterLevel.Linear_Cubic)
            {
                SetCanvasAttr(CanvasAttr.ColorAdjust_Cubic);
            }
            else
            {
                SetCanvasAttr(CanvasAttr.ColorAdjust);
            }
        }
        else
        {
            SetCanvasAttr(CanvasAttr.None);
        }

        Quad SourceQuad = new Quad(CropRect);
        Vector2 Size = new Vector2(CropRect.Right - CropRect.Left, CropRect.Bottom - CropRect.Top);
        Quad DestQuad = new Quad(X, Y, Size.X, Size.Y);

        Vector2 Center;
        if (DoCenter)
            Center = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
        else
            Center = new Vector2(0, 0);

        DestQuad = Rotated2(new Vector2(X, Y), Size, Center, Angle, ScaleX, ScaleY);

        if (FlipX)
            SourceQuad = SourceQuad.Mirror;
        if (FlipY)
            SourceQuad = SourceQuad.Flip;
        if (Hue != 0 || Saturation != 0 || Lightness != 0)
            Game.Canvas.QuadRegion(Texture, DestQuad, new ColorRect(UInt.HSLColor(Hue, Saturation, Lightness)), SourceQuad, BlendingEffect);
        else
            Game.Canvas.QuadRegion(Texture, DestQuad, new ColorRect(Color), SourceQuad, BlendingEffect);
    }

    public static void DrawPattern(Texture Texture, float X, float Y, int PatternIndex, int PatternWidth, int PatternHeight,
      bool FlipX = false, bool FlipY = false, float Angle = 0, bool DoCenter = false, float ScaleX = 1, float ScaleY = 1,
      uint Color = 0xFFFFFFFFu, int Hue = 0, int Saturation = 0, int Lightness = 0, 
      BlendingEffect BlendingEffect = BlendingEffect.Normal, FilterLevel FilterLevel = FilterLevel.None)
    {
        TextureParameters Parameters = Texture.Parameters;

        SetFilterLevel(FilterLevel);
        if (Hue != 0 || Saturation != 0 || Lightness != 0)
        {
            if (FilterLevel == FilterLevel.Linear_Cubic)
            {
                SetCanvasAttr(CanvasAttr.ColorAdjust_Cubic);
            }
            else
            {
                SetCanvasAttr(CanvasAttr.ColorAdjust);
            }
        }
        else
        {
            SetCanvasAttr(CanvasAttr.None);
        }

        int TexWidth = Parameters.Width;
        int TexHeight = Parameters.Height;
        int ColCount = TexWidth / PatternWidth;
        int RowCount = TexHeight / PatternHeight;
        int FPatternIndex = PatternIndex;
        if (FPatternIndex < 0)
            FPatternIndex = 0;
        if (FPatternIndex >= RowCount * ColCount)
            FPatternIndex = RowCount * ColCount - 1;

        int Left = (FPatternIndex % ColCount) * PatternWidth;
        int Right = Left + PatternWidth;
        int Top = (FPatternIndex / ColCount) * PatternHeight;
        int Bottom = Top + PatternHeight;
        int FWidth = Right - Left;
        int FHeight = Bottom - Top;
        int X1 = Left;
        int Y1 = Top;
        int X2 = (Left + FWidth);
        int Y2 = (Top + FHeight);

        Quad SourceQuad = new Quad(Rect.Bounds(X1, Y1, X2, Y2));
        Vector2 Size = new Vector2(PatternWidth, PatternHeight); ;
        Vector2 Center;
        if (DoCenter)
            Center = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
        else
            Center = new Vector2(0, 0);

        Quad DestQuad = Rotated2(new Vector2(X, Y), Size, Center, Angle, ScaleX, ScaleY);
        
        if (FlipX)
            SourceQuad = SourceQuad.Mirror;
        if (FlipY)
            SourceQuad = SourceQuad.Flip;
        if (Hue != 0 || Saturation != 0 || Lightness != 0)
            Game.Canvas.QuadRegion(Texture, DestQuad, new ColorRect(UInt.HSLColor(Hue, Saturation, Lightness)), SourceQuad, BlendingEffect);
        else
            Game.Canvas.QuadRegion(Texture, DestQuad, new ColorRect(Color), SourceQuad, BlendingEffect);
    }

    public static Quad Rotated2(Vector2 origin, Vector2 size, Vector2 center, float angle,
      float ScaleX = 1.0f, float ScaleY = 1.0f)
    {
        float sinAngle = (float)Math.Sin(angle);
        float cosAngle = (float)Math.Cos(angle);

        Vector2[] values = new Vector2[] {
        new(-center.X, -center.Y),
        new(size.X - center.X, -center.Y),
        new(size.X - center.X, size.Y - center.Y),
        new(-center.X, size.Y - center.Y)};

        for (int i = 0; i < values.Length; i++)
        {
            values[i].X *= ScaleX;
            values[i].Y *= ScaleY;
            Vector2 newValue = new(
              values[i].X * cosAngle - values[i].Y * sinAngle,
              values[i].X * sinAngle + values[i].Y * cosAngle);

            values[i] = newValue + origin;
        }
        return new Quad(values[0], values[1], values[2], values[3]);
    }

    public static void DrawEx(Texture Texture, float X, float Y, bool FlipX = false, bool FlipY = false,
      float Angle = 0, bool DoCenter = false, float ScaleX = 1, float ScaleY = 1,
      uint Color = 0xFFFFFFFFu, int Hue = 0, int Saturation = 0, int Lightness = 0, 
      BlendingEffect BlendingEffect = BlendingEffect.Normal, FilterLevel FilterLevel = FilterLevel.None)
    {
       
        SetFilterLevel(FilterLevel);
        if (Hue != 0 || Saturation != 0 || Lightness != 0)
        {
            if (FilterLevel == FilterLevel.Linear_Cubic)
            {
                SetCanvasAttr(CanvasAttr.ColorAdjust_Cubic);
            }
            else
            {
                SetCanvasAttr(CanvasAttr.ColorAdjust);
            }
        }
        else
        {
            SetCanvasAttr(CanvasAttr.None);
        }
       
        TextureParameters Parameters = Texture.Parameters;
        Vector2 Size = new Vector2(Parameters.Width, Parameters.Height); ;
        Vector2 Center;
        if (DoCenter)
            Center = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
        else
            Center = new Vector2(0, 0);

        Quad DestQuad = Rotated2(new Vector2(X, Y), Size, Center, Angle, ScaleX, ScaleY);
        Quad SourceQuad = Quad.Unity;
        if (FlipX)
            SourceQuad = SourceQuad.Mirror;
        if (FlipY)
            SourceQuad = SourceQuad.Flip;
        if (Hue != 0 || Saturation != 0 || Lightness != 0)
            Game.Canvas.Quad(Texture, DestQuad, new ColorRect(UInt.HSLColor(Hue, Saturation, Lightness)), SourceQuad, BlendingEffect);
        else
            Game.Canvas.Quad(Texture, DestQuad, new ColorRect(Color), SourceQuad, BlendingEffect);
    }
    public static void DrawStretch(Texture Texture, float X, float Y, int Width, int Height, BlendingEffect BlendingEffect = BlendingEffect.Normal)
    {
        Game.Canvas.Attributes = 0;
        Quad Quad = new Quad(X, Y, Width, Height);
        Quad Quad2 = Quad.Unity;
        Game.Canvas.Quad(Texture, Quad, null, null, BlendingEffect);
    }

    public static void DrawRotateC(Texture Texture, float X, float Y, float Angle, BlendingEffect BlendingEffect = BlendingEffect.Normal)
    {
        TextureParameters Parameters = Texture.Parameters;
        Game.Canvas.Attributes = 0;
        Vector2 Size = new Vector2(Parameters.Width, Parameters.Height); ;
        Quad DestQuad = Quad.Rotated(new Vector2(X, Y), Size, Angle);
        Game.Canvas.Quad(Texture, DestQuad, null, null, BlendingEffect);
    }

    public static void Draw(Texture Texture, float X, float Y, bool FlipX, bool FlipY, BlendingEffect BlendingEffect = BlendingEffect.Normal)
    {
        TextureParameters Parameters = Texture.Parameters;
        Game.Canvas.Attributes = 0;
        Quad DestQuad = new Quad(X, Y, Parameters.Width, Parameters.Height);
        Quad SourceQuad = Quad.Unity;
        if (FlipX)
            SourceQuad = SourceQuad.Mirror;
        if (FlipY)
            SourceQuad = SourceQuad.Flip;
        Game.Canvas.Quad(Texture, DestQuad, null, SourceQuad, BlendingEffect);
    }
    public static void DrawHSL(Texture Texture, float X, float Y, int Hue, int Saturation, int Lightness, BlendingEffect BlendingEffect = BlendingEffect.Normal)
    {
        TextureParameters Parameters = Texture.Parameters;
        Game.Canvas.Attributes = Canvas.Attribute.ColorAdjust;
        Game.Canvas.Quad(Texture, new Quad(X, Y, Parameters.Width, Parameters.Height),
          new ColorRect(UInt.HSLColor(Hue, Saturation, Lightness)), null, BlendingEffect);

    }

}


