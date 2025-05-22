using Afterwarp.SpriteEngine;

namespace SpriteSheet;

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
        GameFunc.CeateGame();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(0xFFE1E2E6u);
    }
}
