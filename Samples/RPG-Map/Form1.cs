using Afterwarp.SpriteEngine;

namespace RPG_Map;

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
        MapObj.CreateMap();
        Player Player = new Player(Game.SpriteEngine);
        Player.Init("player.png", 1800, 1000);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Game.Draw(0);        
        
    }
}
