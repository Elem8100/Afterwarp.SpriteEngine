using Afterwarp.SpriteEngine;
namespace Basic;

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
        Sprites.Create();
       
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(0);
    }
}
