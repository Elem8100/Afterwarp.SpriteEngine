using Afterwarp.SpriteEngine;

namespace Tiled_Slope;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        Game.Init(4,false);
        Game.LoadTextrues("Images/");
        GameFunc.CreateGame();
    }
    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(UInt.ARGB(255,0,195,205));
    }
}
