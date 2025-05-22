using Afterwarp.SpriteEngine;
namespace Animation;

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
        Game.Draw(0);
    }
}
