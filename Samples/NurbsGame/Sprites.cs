using Afterwarp.SpriteEngine;
using System.Numerics;
using Afterwarp;

namespace NurbsGame;
public class Player : SpriteEx
{
    public Player(Sprite Parent) : base(Parent)
    {
        DoCenter = true;
    }
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        LookAt(MouseEx.X, MouseEx.Y);
    }
}

public class SpaceBall : NPathSprite
{
    public SpaceBall(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.Single;
    }
    public int ID;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        CollidePos = new Vector2(X + 31, Y + 31);
        // if (Distance >= 100) Dead();
        LookAt(3.14159f / 2);
        if (Y > 450 && X > 375)
        {
            Dead();
        }
    }
}

public class ShotBall : PlayerSprite
{
    public ShotBall(Sprite Parent) : base(Parent)
    {
        Random Random = new Random();
        switch (Random.Next(0, 4))
        {
            case 0: ImageName = "Ball0.png"; break;
            case 1: ImageName = "Ball1.png"; break;
            case 2: ImageName = "Ball2.png"; break;
            case 3: ImageName = "Ball3.png"; break;
        }
        CanCollision = true;
        CollideMode = CollideMode.Circle;
        CollideRadius = 30;
        Velocity = 15;
        LifeTime = Velocity;
        Decay = LifeTime / 100;
        Fired = false;
        Angle = Sprites.Player.Angle;
        X = Sprites.Player.X;
        Y = Sprites.Player.Y;
        Z = 100;
        CanCollision = true;
        CollideMode = CollideMode.Circle;
        DoCenter = true;
    }

    public bool Fired;
    public float Velocity;
    public float Decay;
    public float LifeTime;
    public void MoveOut()
    {
        this.Dead();
        Sprites.CanCharge = true;
    }
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Sprites.CanCharge = false;
        if (!Fired)
        {
            Angle = Sprites.Player.Angle;
            X = Sprites.Player.X;
            Y = Sprites.Player.Y;
        }
        else
        {
            X += (float)Math.Cos(Angle - 3.14159 / 2) * Velocity * Delta;
            Y += (float)Math.Sin(Angle - 3.14159 / 2) * Velocity * Delta;

            CollidePos = new Vector2(X + 31, Y + 31);
            if ((X < -20) || (Y < -20) || (X > 900) || (Y > 720))
            {
                MoveOut();
            }

            Collision();
        }
    }
    public override void OnCollision(Sprite sprite)
    {
        if (sprite is SpaceBall)
        {
            var SpaceBall = (SpaceBall)sprite;
            if (SpaceBall.ImageName == this.ImageName)
            {
                SpaceBall.CanCollision = false;
                SpaceBall.Visible = false;
                this.CanCollision = false;
                this.MoveOut();
                Sprites.CreateSpark(SpaceBall.X, SpaceBall.Y, SpaceBall.ImageName.Substring(4, 1));
            }
            else
            {
                Sprites.CanCharge = true;
                CanCollision = false;
                Velocity = 10;
                Direction = (int)(40.76f * Angle);

                if (Direction > 128 && Direction < 210)
                {

                    TowardToAngle(Direction, 0, true, 1);
                }
                else
                {
                    FlipXDirection();
                    FlipYDirection();
                    TowardToAngle(Direction, 0, true, 1);
                }
            }
        }
    }

    public void SwitchColor()
    {
        switch (ImageName)
        {
            case "Ball0.png": ImageName = "Ball1.png"; return; break;
            case "Ball1.png": ImageName = "Ball2.png"; return; break;
            case "Ball2.png": ImageName = "Ball3.png"; return; break;
            case "Ball3.png": ImageName = "Ball0.png"; return; break;
        }
    }
}

public class Spark : PlayerSprite
{
    public Spark(Sprite Parent) : base(Parent)
    {
    }
    float _Alpha=255;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Accelerate(Delta);
        UpdatePos(Delta);

        _Alpha-=4*Delta;

        Alpha= (byte)MathF.Round(_Alpha);
      //  OnTimer(17, () => 
       // {
         //   Alpha -= 4;
      //  });

        if (Alpha < 10)
            Dead();
    }
}

public class Sprites
{
    public static Player Player;
    public static ShotBall ShotBall;
    public static bool CanCharge;
    public static float NextBallInterval;
    public static int GameBallCount;
    public static bool NextBallReady;
    public static NURBSCurveEx LevelPath;
    public static void ChargeShot()
    {
        ShotBall = new ShotBall(Game.SpriteEngine);
        ShotBall.ImageName = ShotBall.ImageName;
        CanCharge = false;
    }
    public static void CreateSpark(float PosX, float PosY, string Num)
    {
        var Random = new Random();
        for (int i = 0; i < 128; i++)
        {
            var Spark = new Spark(Game.SpriteEngine);
            Spark.SpriteSheetMode = SpriteSheetMode.Single;
            Spark.ImageName = "Spark" + Num + ".png";
            Spark.X = PosX + -Random.Next(0, 31);
            Spark.Y = -20 + PosY + Random.Next(0, 31);
            Spark.Z = 200;
            Spark.BlendingEffect = BlendingEffect.Add;
            Spark.Acceleration = 0.02f;
            Spark.MinSpeed = 1.8f;
            Spark.MaxSpeed = (float)-(0.4 + Random.Next(0, 3));
            Spark.Direction = i * 2;
        }
    }
    public static void Create()
    {
        Player = new Player(Game.SpriteEngine);
        Player.ImageName = "Cannon.png";
        Player.X = 820;
        Player.Y = 600;
        LevelPath = new NURBSCurveEx();
        LevelPath.FittingCurveType = FittingCurveType.ConstantSpeed;
        LevelPath.LoadCurve("NURBS.txt");
        NextBallReady = true;
        CanCharge = true;
    }
    public static void Update()
    {
        if (MouseEx.LeftClick)
        {
            if (!ShotBall.Fired)
            {
                ShotBall.Fired = true;
                ShotBall.Z = 0;
            }
        }

        if (MouseEx.RightClick)
        {
            if (!ShotBall.Fired)
                ShotBall.SwitchColor();
        }
        NextBallInterval = 1.8f;
        var SpriteList = Game.SpriteEngine.SpriteList;

        foreach (var Sprite in SpriteList)
        {
            if ((Sprite is SpaceBall) && ((SpaceBall)Sprite).ID == GameBallCount - 1)
            {
                if (((SpaceBall)Sprite).Distance >= NextBallInterval)
                {
                    NextBallReady = true;
                }
            }
        }

        if (NextBallReady)
        {
            var SpaceBall = new SpaceBall(Game.SpriteEngine);
            SpaceBall.ImageName = "Ball0.png";
            SpaceBall.X = 0;
            SpaceBall.Y = 0;

            SpaceBall.ID = GameBallCount;
            Random Random = new Random();
            switch (Random.Next(0, 4))
            {
                case 0: SpaceBall.ImageName = "Ball0.png"; break;
                case 1: SpaceBall.ImageName = "Ball1.png"; break;
                case 2: SpaceBall.ImageName = "Ball2.png"; break;
                case 3: SpaceBall.ImageName = "Ball3.png"; break;
            }
            SpaceBall.MoveSpeed = 1.5f;
            SpaceBall.Path = LevelPath;
            SpaceBall.X = SpaceBall.Path.GetXY(0).X;
            SpaceBall.Y = SpaceBall.Path.GetXY(0).Y;
            SpaceBall.Offset.X = 33;
            SpaceBall.Offset.Y = -33;
            SpaceBall.CanCollision = true;
            SpaceBall.CollideMode = CollideMode.Circle;
            SpaceBall.CollideRadius = 31;
            NextBallReady = false;
            GameBallCount++;
        }
        if (CanCharge)
            ChargeShot();

    }
}