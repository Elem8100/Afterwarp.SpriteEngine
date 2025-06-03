using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afterwarp.SpriteEngine;

public class SpotLight
{
    public SpotLight(int size, float scaleY, int offsetX, int offsetY)
    {
        Size = size;
        ScaleY = scaleY;
        OffsetX = offsetX;
        OffsetY = offsetY;
    }
    public int Size;
    public float ScaleY;
    public int OffsetX, OffsetY;
    public SpriteEx Owner;
    public static List<SpotLight> List = new();

    public void Draw(float X, float Y, int Size, float ScaleY = 1)
    {
        float Width = (float)Size / 256;
        float Height = Size * (float)ScaleY / 256;
        GameCanvas.DrawEx(LightTexture, X, Y, false, false, 0, true, Width, Height, UInt.ARGB(250, 255, 255, 255), 0, 0, 0,
          BlendingEffect.Normal, FilterLevel.Linear);
        GameCanvas.DrawEx(LightTexture, X, Y, false, false, 0, true, Width, Height, UInt.ARGB(250, 255, 255, 255), 0, 0, -15,
         BlendingEffect.Add, FilterLevel.Linear);
    }

    static Texture LightTexture;
    public static Texture Surface;
    static bool HasCreate = false;

    public static void DrawRenderTarget(int Alpha)
    {
        if (!HasCreate)
        {
            TextureParameters parameters = default;
            parameters.Width = 256;
            parameters.Height = 256;
            parameters.Attributes = Texture.Attribute.Drawable | Texture.Attribute.PremultipliedAlpha;
            parameters.Format = PixelFormat.RGBA16;
            parameters.Multisamples = 8;
            LightTexture = new Texture(Game.Device, parameters);
            LightTexture.Clear(new FloatColor(UInt.ARGB(0, 0, 0, 0)));
            LightTexture.Begin();
            Game.Canvas.Begin();
            Game.Canvas.Highlight(new RectF(128, 128, 0, 0), 0f, 1f, 128, 1);
            Game.Canvas.End();
            LightTexture.End();
            //
            parameters.Width = 1200;
            parameters.Height = 1200;
            Surface = new Texture(Game.Device, parameters);
            HasCreate = true;
        }

        Surface.Clear();
        Surface.Begin();
        Game.Canvas.Begin();
        Game.Canvas.FillRect(new RectF(0, 0, 2048, 2048), new ColorRect(UInt.ARGB(Alpha, 255, 255, 255)));

        foreach (var Iter in SpotLight.List)
        {
            Iter.Draw(Iter.Owner.X - Game.SpriteEngine.Camera.X + Iter.OffsetX,
                      Iter.Owner.Y - Game.SpriteEngine.Camera.Y + Iter.OffsetY,
                      Iter.Size, Iter.ScaleY);
        }
        Game.Canvas.End();
        Surface.End();
    }

    public static void DrawOnScreen(int X, int Y)
    {
        GameCanvas.Draw(SpotLight.Surface, X, Y, Afterwarp.BlendingEffect.Multiply);
    }


}

