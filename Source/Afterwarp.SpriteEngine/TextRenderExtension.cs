using System.Numerics;
using Afterwarp;
namespace Afterwarp.SpriteEngine;
public static class TextRenderExtension
{
    static string _Family;
    static float _Size;
    static FontWeight _FontWeight;
    static float _FillBrightness;
    static float _FillOpacity;
    static FontStretch _FontStretch;
    static FontSlant _FontSlant;
    public static void New(this Afterwarp.TextRenderer TextRenderer, string Family, float Size, FontWeight FontWeight = FontWeight.Normal,
      float FillBrightness = 1, float FillOpacity = 1, FontStretch FontStretch = FontStretch.Normal, FontSlant FontSlant = FontSlant.None, byte Attributes = 0)
    {
        FontParameters Parameters = new(Family, Size, FontWeight, FontStretch, FontSlant, Attributes);
        Parameters.Effect.FillBrightness = FillBrightness;
        Parameters.Effect.FillOpacity = FillOpacity;
        TextRenderer.Parameters = Parameters;

        _Family = Family;
        _Size = Size;
        _FontWeight = FontWeight;
        _FillBrightness = FillBrightness;
        _FillOpacity = FillOpacity;
        _FontStretch = FontStretch;
        _FontSlant = FontSlant;
    }

    public static void SetBorder(this Afterwarp.TextRenderer TextRenderer, FontBorder BorderType, float BorderBrightness = 0.25f,
       float BorderOpacity = 0.75f, float BorderThickness = 1f)
    {
        FontParameters Parameters = new(_Family, _Size, _FontWeight, _FontStretch, _FontSlant);
        Parameters.Effect.FillBrightness = _FillBrightness;
        Parameters.Effect.FillOpacity = _FillOpacity;

        Parameters.Effect.BorderType = BorderType;
        Parameters.Effect.BorderBrightness = BorderBrightness;
        Parameters.Effect.BorderOpacity = BorderOpacity;
        Parameters.Effect.BorderThickness = BorderThickness;
        TextRenderer.Parameters = Parameters;
    }

    public static void SetShadow(this Afterwarp.TextRenderer TextRenderer, float ShadowBrightness = 0.15f, float ShadowOpacity = 0.75f,
      Vector2 ShadowDistance = default, float ShadowSmoothness = 3)
    {
        FontParameters Parameters = new(_Family, _Size, _FontWeight, _FontStretch, _FontSlant);
        Parameters.Effect.FillBrightness = _FillBrightness;
        Parameters.Effect.FillOpacity = _FillOpacity;

        Parameters.Effect.ShadowBrightness = ShadowBrightness;
        Parameters.Effect.ShadowOpacity = ShadowOpacity;
        Parameters.Effect.ShadowDistance = ShadowDistance;
        Parameters.Effect.ShadowSmoothness = ShadowSmoothness;
        TextRenderer.Parameters = Parameters;
    }

    public static void Draw(this Afterwarp.TextRenderer TextRenderer, float X, float Y, string Text, uint Color, float Alpha = 1)
    {
        TextRenderer.Draw(new Vector2(X, Y), Text, new ColorPair(Color), Alpha);
    }

    public static void DrawCentered(this Afterwarp.TextRenderer TextRenderer, float X, float Y, string Text, uint Color,
        float Alpha = 1, bool AlignToPixel = true)
    {

        TextRenderer.DrawCentered(new Vector2(X, Y), Text, new ColorPair(Color), Alpha, AlignToPixel);
    }

}
