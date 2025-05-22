using Afterwarp.SpriteEngine;

namespace Z_Order;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Game.Init(4,true);
        Game.LoadTextrues("Images/");
        Sprites.Create();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(UInt.ARGB(255,0,200,200));
    }
}
