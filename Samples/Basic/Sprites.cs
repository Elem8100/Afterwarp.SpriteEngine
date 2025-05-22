using System.Numerics;
using System.Windows.Forms;
using Afterwarp.SpriteEngine;
using Keyboard = Afterwarp.SpriteEngine.Keyboard;
using Keys = Afterwarp.SpriteEngine.Keys;
using Afterwarp;
namespace Basic;
public class PlayerSprite : SpriteEx
{
    public PlayerSprite(Sprite Parent) : base(Parent)
    {
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Keyboard.GetState();
        CollidePos = new Vector2(X + 50, Y + 50);
        
        if (Keyboard.KeyDown(Keys.Up))
            Y -= 3 * Delta;
        if (Keyboard.KeyDown(Keys.Down))
            Y += 3 * Delta;
        if (Keyboard.KeyDown(Keys.Left))
            X -= 3 * Delta;
        if (Keyboard.KeyDown(Keys.Right))
            X += 3 * Delta;
        Collision();
        Engine.Camera.X = X - 452;
        Engine.Camera.Y = Y - 300;

        Game.TextRenderer.New("Arial",15);
        Game.TextRenderer.Draw(new Vector2(8.0f , 4.0f),
               $"Frame Rate: {(int)MathF.Round(Game.Timer.FrameRate)}", new ColorPair(0xA0FFFFFFu));

        
    }

    public override void OnCollision(Sprite sprite)
    {
        if (sprite is BallSprite)
        {
            ((BallSprite)(sprite)).ImageName = "img1-2.png";
            ((BallSprite)(sprite)).CanCollision = false;
            ((BallSprite)(sprite)).Hit = true;
        }
    }
}

public class BallSprite : SpriteEx
{
    public BallSprite(Sprite Parent) : base(Parent)
    {
    }
    public float Counter;
    public int Life;
    public bool Hit;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        CollidePos = new Vector2(X + 80, Y + 80);
        Counter += 1 * Delta;
        X += (float)(Sin256((int)Counter)) * 3 * Delta;
        Y += (float)(Cos256((int)Counter)) * 3 * Delta;

       
        if (Hit)
        {
            Life -= 1;
            if (Life < 0)
                Dead();
        }
    }
}

public class Sprites
{
    public static void Create()
    {
        //create tiles
        SpriteEx[,] Tiles = new SpriteEx[80, 80];
        Random Rnd = new Random();
        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                Tiles[i, j] = new SpriteEx(Game.SpriteEngine);
                Tiles[i, j].ImageName = "t" + Rnd.Next(0, 19) + ".png";
                Tiles[i, j].SetSize(64, 64);
                Tiles[i, j].X = i * 64;
                Tiles[i, j].Y = j * 64;
                Tiles[i, j].Z = 1;
                
                Tiles[i, j].IntMove= true;
                Tiles[i, j].FilterLevel = FilterLevel.None;
            }
        }
        //Create Ball
        for (int i = 0; i < 200; i++)
        {
            BallSprite ballSprite = new BallSprite(Game.SpriteEngine);
            ballSprite.ImageName = "img1.png";
            ballSprite.SetSize(200, 200);
            ballSprite.X = Rnd.Next(0, 5000);
            ballSprite.Y = Rnd.Next(0, 5000);
            ballSprite.CanCollision = true;
            ballSprite.CollideRadius = 80;
            ballSprite.Counter = Rnd.Next(0, 1000);
            ballSprite.Life = 15;
            ballSprite.Hit = false;
        }
        //Create Player
        PlayerSprite PlayerSprite1 = new PlayerSprite(Game.SpriteEngine);
        PlayerSprite1.ImageName = "img2.png";
        PlayerSprite1.X = 2500;
        PlayerSprite1.Y = 2500;
        PlayerSprite1.Z = 10;
        PlayerSprite1.CanCollision = true;
        PlayerSprite1.CollideRadius = 50;
    }

}