using Afterwarp.SpriteEngine;
using Keyboard = Afterwarp.SpriteEngine.Keyboard;
using Keys = Afterwarp.SpriteEngine.Keys;
using System.Numerics;

using Afterwarp;

namespace Shooter;
public class Bullet : SpriteEx
{
    public Bullet(Sprite Parent) : base(Parent)
    {
        BlendingEffect= BlendingEffect.Add;
        CanCollision = true;
        CollideRadius = 9;
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Y -= 9 * Delta;
        if (Y < 0)
        {
            Dead();
            Visible = false;
        }
        CollidePos = new Vector2(X + 25, Y + 10);
        Collision();
    }

    public override void OnCollision(Sprite sprite)
    {
        if (sprite is Enemy)
        {
            CanCollision = false;
            Dead();
            var Enemy = (Enemy)sprite;
            Enemy.BlendingEffect =  BlendingEffect.Add;
            Enemy.SetPattern(64, 64);
            Enemy.SetAnim("explode.png", 0, 32, 0.4f, false, false, true);
            Enemy.ScaleX = 1.5f;
            Enemy.ScaleY = 1.5f;
            Enemy.Shadow.Dead();
        }
    }
}

public class Enemy : AnimatedSprite
{
    public Enemy(Sprite Parent) : base(Parent)
    {
       
        SetPattern(96, 96);
        CollideRadius = 28;
        CanCollision = true;
     
    }
    public float MoveSpeed;
    public int AI;
    public float Velocity1 = 1;
    public float Velocity2 = 2;
    public float Curve;
    public float OriginalX;
    public AnimatedSprite Shadow;
    public override void DoMove(float Delta)
    {
      
        base.DoMove(Delta);
       
        CollidePos = new Vector2(X + 53, Y + 48);
        if (ImageName == "explode.png")
        {
            CanCollision = false;
            Velocity1 = 0;
            Velocity2 = 0;
            if (AnimPos == 31)
                Dead();
        }
        
        switch (AI)
        {
            case 0:
                Y += Velocity2 * Delta; break;

            case 1:
            case 2:
                Y += Velocity2 * Delta;
                Curve += (Velocity1*Delta);
                X = OriginalX + (int)(150 * Math.Sin(3 * 3.1615926 * Curve / 360));
                break;
            case 3:
                Y += Velocity2 * Delta;
                X += Velocity1 * Delta;
                break;

            case 4:
                Y += Velocity2 * Delta;
                X -= Velocity1 * Delta;
                break;
        }

        Shadow.ImageName = ImageName;
        Shadow.X = X + 25;
        Shadow.Y = Y + 25;
        Shadow.Z = Z - 1;
        Shadow.PatternIndex = PatternIndex;
        Shadow.AnimPos = AnimPos;
        Shadow.SetColor(0, 0, 0, 60);

        if (Y > 1000)
        {
            Dead();
            Shadow.Dead();
            CanCollision = false;
        }
    }

    public static void CreateEnemy()
    {
        Random Random = new Random();
        for (int i = 0; i <= 3; i++)
        {
            if (Random.Next(0, 100) == 50)
            {
                var Enemy = new Enemy(Game.SpriteEngine);
                    
                Enemy.X = Random.Next(-10, 1000);
                Enemy.Y = -Random.Next(0, 200);
                Enemy.OriginalX = Enemy.X;
                var ImageName = "enemy" + Random.Next(0, 3).ToString() + ".png";
                Enemy.SetAnim(ImageName, 0, 8, 0.4f, true);
                Enemy.AI = Random.Next(0, 5);
                //
                Enemy.Shadow = new AnimatedSprite(Game.SpriteEngine);
                Enemy.Shadow.ImageName = Enemy.ImageName;
                Enemy.Shadow.SetPattern(96, 96);
               // Enemy.Engine.Move(1);
            }
        }
    }

}

public class Cloud : SpriteEx
{
    public Cloud(Sprite Parent) : base(Parent)
    {
    }
    public float Speed;
    public float Size = 1;
    public float Value = 0.001f;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Y += 1.5f * Delta;
        Z = 10;
        Size += Value * Delta;
        if ((Size > 1) || (Size < 0.9))
            Value = -Value;
        ScaleX = Size;
        if (Y > 900)
            Dead();
    }
    public static void CreateCloud()
    {
        Random Random = new Random();
        for (int i = 0; i <= 3; i++)
        {
            if (Random.Next(0, 780) == 225)
            {
                var Cloud = new Cloud(Game.SpriteEngine);
                
                Cloud.X = Random.Next(-10, 1000);
                Cloud.Y = -Random.Next(200, 400);
                Cloud.FlipX = Convert.ToBoolean(Random.Next(0, 2));
                Cloud.FlipY = Convert.ToBoolean(Random.Next(0, 2));
                Cloud.Height = 400;
                var ImageName = "cloud" + Random.Next(0, 2).ToString() + ".png";
                Cloud.ImageName = ImageName;
                Cloud.BlendingEffect = BlendingEffect.Add;
                Cloud.SetColor(100, 100, 100, 255);
            }
        }
    }
}

public class BaseShip : AnimatedSprite
{
    public BaseShip(Sprite Parent) : base(Parent)
    {
        PatternWidth = 128;
        PatternHeight = 128;
    }
    public float MoveSpeed = 3f;
    private bool IsPress = true;
    public override void DoMove(float Delta)
    {
       
        base.DoMove(Delta);
        Keyboard.GetState();
       
        if (Keyboard.KeyDown(Keys.Right))
        {
            X += MoveSpeed * Delta;
            if (X > 1000) X = 1000;
        }

        if ((IsPress))
        {
            if (Keyboard.KeyDown(Keys.Right))
            {
                X -= 1;
                AnimPos = 0;
                PatternIndex = 0;
                SetAnim("TurnRight.png", 0, 4, 0.25f, false, false, true);
                IsPress = false;
            }
        }

        if (Keyboard.KeyUp(Keys.Right))
        {
            X -= 3 * Delta;
            SetAnim("TurnLeft.png", 0, 4, 0.2f, false, false, true);
            PatternIndex = 0;
            AnimPos = 0;
            IsPress = true;
        }

        if (Keyboard.KeyDown(Keys.Left))
        {
            X -= MoveSpeed * Delta;
            if (X < 0) X = 0;
        }

        if ((IsPress))
        {
            if (Keyboard.KeyDown(Keys.Left))
            {
                X += 1;
                if (X > 1000) X = 1000;
                AnimPos = 0;
                PatternIndex = 0;
                SetAnim("TurnRight.png", 0, 4, 0.25f, false, true, true);
                IsPress = false;
            }
        }

        if (Keyboard.KeyUp(Keys.Left))
        {
            X += 1;
            SetAnim("TurnLeft.png", 0, 4, 0.2f, false, true, true);
            PatternIndex = 0;
            AnimPos = 0;
            IsPress = true;
        }

        if (Keyboard.KeyDown(Keys.Up))
        {
            Y -= MoveSpeed * Delta;
            if (Y < -40)
                Y = -40;
        }
        if (Keyboard.KeyDown(Keys.Down))
        {
            Y += MoveSpeed * Delta;
            if (Y > 800)
                Y = 800;
        }
    }
}

public class Ship : BaseShip
{
    public Ship(Sprite Parent) : base(Parent)
    {

    }
    public AnimatedSprite Tail;
    public AnimatedSprite Shadow;
    public void CreateShadow()
    {
        Shadow = new AnimatedSprite(this.Engine);
        Shadow.Init(ImageName, 0, 0, 0, 128, 128);
        Shadow.SetColor(0, 0, 0, 60);
        //
        Tail = new AnimatedSprite(this.Engine);
        Tail.Init( "tail.png", 0, 0, 0, 40, 20);
        Tail.ScaleX = 0.8f;
        Tail.BlendingEffect = BlendingEffect.Add;
        Tail.SetAnim("tail.png", 0, 3, 0.2f, true);
    }

    public override void DoMove(float MoveCount)
    {
        base.DoMove(MoveCount);
        if (Shadow != null)
        {
            Shadow.ImageName = this.ImageName;
            Shadow.FlipX = this.FlipX;
            Shadow.AnimPos = this.AnimPos;
            Shadow.PatternIndex = this.PatternIndex;
            Shadow.X = this.X + 30;
            Shadow.Y = this.Y + 25;
            Shadow.Z = this.Z - 1;
        }

        if (Tail != null)
        {
            Tail.X = this.X + 48;
            Tail.Y = this.Y + 115;
        }

        if (Keyboard.KeyPressed(Keys.Space))
        {
            for (int i = 0; i <= 1; i++)
            {
                var Bullet = new Bullet(this.Engine);
                
                Bullet.ImageName = "bullet.png";
                Bullet.X = this.X + 25 + i * 25;
                Bullet.Y = this.Y;
            }
        }

    }
}

public class GameFunc
{
    public static BackgroundSprite Background;
    public static void Create()
    {
        Background = new BackgroundSprite(Game.SpriteEngine);
        Background.Init("jungle.png", 0, 0, 0, 0, 0, 1000, 1279);
        Background.TileMode = TileMode.Vertical;
        var Ship = new Ship(Game.SpriteEngine);
        Ship.Init("TurnRight.png", 400, 600, 5);
        Ship.CreateShadow();
    }

}