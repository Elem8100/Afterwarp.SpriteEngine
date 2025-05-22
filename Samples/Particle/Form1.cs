using Afterwarp;
using Afterwarp.SpriteEngine;
using System.Diagnostics;
using System.Numerics;
namespace Particle;

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
        Sprites.CreateScanline();
    }

    TimerEx TimerEx = new TimerEx();

    private void Form1_Paint(object sender, PaintEventArgs e)
    {

        TimerEx.OnTimer(40, () =>
        {
            Sprites.CreateParticle();
        });

        Game.Draw(4294967280u, () =>
        {
            Game.SpriteEngine.Draw();
            Game.SpriteEngine.Move(Game.Timer.Latency * 0.00006f);
            Game.SpriteEngine.Dead();

            Game.TextRenderer.New("Arial", 25.0f, FontWeight.Bold);
            Game.TextRenderer.SetBorder(FontBorder.Heavy, 0f, 1);
            Game.TextRenderer.SetShadow(0.01f, 1, new Vector2(1f, 1f));
            Game.TextRenderer.DrawAligned(new Vector2(50, 50), "FPS: " + (int)MathF.Round(Game.Timer.FrameRate),
                new ColorPair(0xFF75C916u, 0xFF9CFADCu), TextAlignment.Start);
        });
    }
}
