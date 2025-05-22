using Afterwarp.SpriteEngine;

namespace ChunLi;

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
        Game.Draw(UInt.ARGB(255,0,190,250));
    }
}
