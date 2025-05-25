using Afterwarp.SpriteEngine;
using Afterwarp;
using System.Numerics;
namespace HSL_Color;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }
    
    private void Form1_Load(object sender, EventArgs e)
    {
        Game.Init();
        Game.LoadTextrues("Images/");
    }
    float Hue;
    float SatValue,SatStep,BrightValue;
    float Angle;
    
    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        float TimeDelta = Game.Timer.Latency * 0.00006f;
        
        Angle += 0.005f * TimeDelta;
        Hue += 1 * TimeDelta;
        if (Hue > 180)
            Hue = -180;

        SatValue += SatStep * TimeDelta;
        float Saturation = (float)Math.Sin(SatValue) * 100;
        if (Saturation<-90 )
            SatStep=0.004f;
        else
            SatStep=0.02f;

        BrightValue += 0.02f * TimeDelta;
        float Brightness = (float)Math.Sin(BrightValue) * 100;

        Game.Draw(0xFFE1E2E6u, () =>
        {
            GameCanvas.DrawEx(Game.TextureLib["Ship.png"], 200, 250, false, false, Angle, true, 1, 1, 0xFFFFFFFFu,
              (int)Hue, 0, 0, BlendingEffect.Normal, FilterLevel.Linear);

            GameCanvas.DrawEx(Game.TextureLib["Ship.png"], 520, 250, false, false, Angle, true, 1, 1, 0xFFFFFFFFu,
              0, (int)Saturation, 0, BlendingEffect.Normal, FilterLevel.Linear);

            GameCanvas.DrawEx(Game.TextureLib["Ship.png"], 850, 250, false, false, Angle, true, 1, 1, 0xFFFFFFFFu,
              0, 0, (int)Brightness, BlendingEffect.Normal, FilterLevel.Linear);

            Game.Canvas.Attributes = 0;
            Game.TextRenderer.New("Segoe UI", 16, FontWeight.SemiBold);
            Game.TextRenderer.SetBorder(FontBorder.SemiHeavy);

            Game.TextRenderer.DrawAligned(new Vector2(150, 430), "Hue=" + MathF.Round(Hue).ToString(),
                          new ColorPair(0xFF75C916u, 0xFF9CFADCu), TextAlignment.Start);
            Game.TextRenderer.DrawAligned(new Vector2(450, 430), "Saturation=" + MathF.Round(Saturation).ToString(),
                        new ColorPair(0xFF75C916u, 0xFF9CFADCu), TextAlignment.Start);
            Game.TextRenderer.DrawAligned(new Vector2(770, 430), "Brightness=" + MathF.Round(Brightness).ToString(),
                      new ColorPair(0xFF75C916u, 0xFF9CFADCu), TextAlignment.Start);

            Game.TextRenderer.DrawAligned(new Vector2(20, 20), "FPS: " + MathF.Round(Game.Timer.FrameRate).ToString(),
                     new ColorPair(0xFF3318D6u, 0xFFFDC3E7u), TextAlignment.Start);

        });
       
    }
}
