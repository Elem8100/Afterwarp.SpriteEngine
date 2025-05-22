using Afterwarp.SpriteEngine;

namespace Platformer;

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
        GameFunc.CreateMap();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(0, () =>
        {
            Game.BackgroundEngine.Draw();
            Game.BackgroundEngine.Move(Game.Timer.Latency * 0.00006f);
            
            Game.SpriteEngine.Draw();
            Game.SpriteEngine.Move(Game.Timer.Latency * 0.00006f);
            Game.SpriteEngine.Dead();
            
        });

    }
}
