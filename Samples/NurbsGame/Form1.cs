using Afterwarp.SpriteEngine;
namespace NurbsGame;
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
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(0, () =>
        {
            GameCanvas.Draw(Game.TextureLib["Back.jpg"], 0, 0);
            Game.SpriteEngine.Draw();
            Game.SpriteEngine.Move(Game.Timer.Latency * 0.00006f);
            Game.SpriteEngine.Dead();
            Sprites.Update();
        });
    }
}
