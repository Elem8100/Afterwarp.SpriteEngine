using Afterwarp.SpriteEngine;
namespace Spot_Light;

public class Ant : PlayerSprite
{
    public Ant(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.Pattern;
        SetPattern(101, 124);
        SetAnim("ant.png", 0, 62, 3f, true);
        DoCenter = true;
        AddSpotLight(200, 1, 0, 0);
    }
    int Rx, Ry;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Random Rnd = new Random();

        OnTimer(50, () =>
        {
            if (Rnd.Next(1, 20) == 10)
            {
                Rx = Rnd.Next(1, 1000);
                Ry = Rnd.Next(1, 890);

            }
        });

        if (X > 1050 || X < 120 || Y > 820 || Y < 100)
        {
            Rx = Rnd.Next(1, 1050);
            Ry = Rnd.Next(1, 890);
        }

        RotateToPos(Rx, Ry, 0.85f, 2, Delta);
    }
}

public class Finger : PlayerSprite
{
    public Finger(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.Single;
        ImageName = "Finger.png";
        Z = 10;
        AddSpotLight(300,1,0,0);
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        X = MouseEx.X;
        Y = MouseEx.Y;
    }
}
public class Sprites
{
    public static void Create()
    {
        Random Rnd = new Random();
        //Create Ants
        for (int i = 0; i < 6; i++)
        {
            Ant Ant = new Ant(Game.SpriteEngine);
            Ant.X = Rnd.Next(0, 1100);
            Ant.Y = Rnd.Next(0, 800);
        }
        //Create finger
        Finger Finger = new Finger(Game.SpriteEngine);
    }

}
