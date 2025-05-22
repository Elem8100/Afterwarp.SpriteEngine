using Afterwarp.SpriteEngine;

namespace Shooter;

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
        GameFunc.Create();

    }
    TimerEx TimerEx=new TimerEx();
    private void Form1_Paint(object sender, PaintEventArgs e)
    {

       

        Game.Draw(0, () =>
        {
           
            TimerEx.OnTimer(17, () =>
            {
                Enemy.CreateEnemy();
                Cloud.CreateCloud();
            });
           
            GameFunc.Background.X = 0;
            GameFunc.Background.Y -= 1f * Game.Timer.Latency*0.00006f;
            Game.SpriteEngine.Draw();
            Game.SpriteEngine.Move(Game.Timer.Latency * 0.00006f);
            Game.SpriteEngine.Dead();
        });

       
    }
}
