using Afterwarp.SpriteEngine;

namespace Spot_Light;

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
        Sprites.Create();
        Cursor.Hide();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(0, () =>
        {
            SpotLight.DrawRenderTarget(80);
            GameCanvas.Draw(Game.TextureLib["back.jpg"], 0, 0);
            Game.SpriteEngine.Draw();
            Game.SpriteEngine.Move(Game.Timer.Latency * 0.00006f);
            SpotLight.DrawOnScreen(0, 0);
        });

    }
}
