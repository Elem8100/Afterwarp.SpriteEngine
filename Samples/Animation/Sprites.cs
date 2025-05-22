using System;
using Afterwarp.SpriteEngine;
using Keyboard = Afterwarp.SpriteEngine.Keyboard;

namespace Animation;
public class BaseSprite : PlayerSprite
{
    public BaseSprite(Sprite Parent) : base(Parent)
    {

    }
    public int FrameCount;
    public void GetFrameCount()
    {
        FrameCount = PatternCount / 5;
    }
    public void PlayAnimation(string ImageName)
    {
        int StartFrame = 0;
        switch (GoDirection)
        {
            case int D when D >= 240 && D < 256:                                                      //       7   0   1 
                                                                                                      //        \  | /
            case int s when s >= 0 && s < 16: StartFrame = 0 * FrameCount; FlipX = false; break;       //        \ |/
            case int D when D >= 16 && D < 48: StartFrame = 1 * FrameCount; FlipX = false; break;      //     6---------2
            case int D when D >= 48 && D < 80: StartFrame = 2 * FrameCount; FlipX = false; break;      //         /|\
            case int D when D >= 80 && D < 112: StartFrame = 3 * FrameCount; FlipX = false; break;     //        / | \
            case int D when D >= 112 && D < 144: StartFrame = 4 * FrameCount; FlipX = false; break;    //       5  4  3
            //5,6,7 use Image Flip                                                                               
            case int D when D >= 144 && D < 176: StartFrame = 3 * FrameCount; FlipX = true; break;
            case int D when D >= 176 && D < 208: StartFrame = 2 * FrameCount; FlipX = true; break;
            case int D when D >= 208 && D < 240: StartFrame = 1 * FrameCount; FlipX = true; break;
        }
        SetAnim(ImageName, StartFrame, FrameCount, 0.25f, true, FlipX, true);
    }
}
public class Player : BaseSprite
{
    public Player(Sprite Parent) : base(Parent)
    {

    }
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        DoAnimate = false;
        if (MouseEx.LeftDown)
        {
            TowardToPos(MouseEx.X + (int)Engine.Camera.X, MouseEx.Y + (int)Engine.Camera.Y, 3, false, false, Delta);
            PlayAnimation("Player.png");
        }
        Engine.Camera.X = X - 550;
        Engine.Camera.Y = Y - 420;

      
    }
}

public class Monster : BaseSprite
{
    public Monster(Sprite Parent) : base(Parent)
    {
        CanCollision = true;
        CollideMode = CollideMode.Rect;
    }
    int RandomX = 0, RandomY = 0;
    public bool ShowName;
   
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Saturation = 0;
        Lightness = 0;

        var Random = new Random();
        OnTimer(20, () => 
        {
            if (Random.Next(1, 25) == 12)
            {
                RandomX = Random.Next((int)X - 10000, (int)X + 10000);
                RandomY = Random.Next((int)Y - 10000, (int)Y + 10000);
            }
        });

        TowardToPos(RandomX, RandomY, 2f, false, false, Delta);
        PlayAnimation(ImageName);
        ShowName = false;
    
        SetCollideRect(0, 0, PatternWidth, PatternHeight);
       // Game.TextRenderer.Draw(100, 100, Game.Timer.FrameRate.ToString(), UInt.ARGB(255, 50, 255, 255));
    }
    public override void DoDraw()
    {
        Game.TextRenderer.New("Arial", 12);
        Game.TextRenderer.SetBorder(Afterwarp.FontBorder.SemiHeavy);
        if (ShowName)
            Game.TextRenderer.Draw((int)(X - Engine.Camera.X), (int)(Y - Engine.Camera.Y - 15), Name, UInt.ARGB(255, 50, 255, 255));
        base.DoDraw();
       
    }
}
public class Finger : Sprite
{
    public Finger(Sprite Parent) : base(Parent)
    {
        CanCollision = true;
        CollideMode = CollideMode.Rect;
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        X = Engine.Camera.X + MouseEx.X;
        Y = Engine.Camera.Y + MouseEx.Y;
        SetCollideRect(0, 0, 31, 26);
        Collision();
    }

    public override void OnCollision(Sprite sprite)
    {
        if (sprite is Monster)
        {
            var Monster = (Monster)sprite;
            Monster.Saturation= 25;
            Monster.Lightness = 20;
            Monster.ShowName = true;
        }
    }
}

public class Sprites
{
    public static void Create()
    {
        //create tiles   
        Random Random = new Random();
        for (int x = 0; x < 80; x++)
        {
            for (int y = 0; y < 80; y++)
            {
                var Tile = new SpriteEx(Game.SpriteEngine);
                Tile.ImageName = "Floor.png";
                Tile.SpriteSheetMode = SpriteSheetMode.Pattern;
                Tile.SetPattern(160, 80);
                Tile.SetSize(160, 80);
               
                Tile.PatternIndex = Random.Next(0, 31);
                Tile.X = 79 * y - 640;
                if ((y % 2) == 0)
                    Tile.Y = 79 * x - 1600;
                if ((y % 2) == 1)
                    Tile.Y = 40 + 79 * x - 1600;
            }
        }
        //create Player
        var Player = new Player(Game.SpriteEngine);
        
        Player.SpriteSheetMode = SpriteSheetMode.Pattern;
        Player.X = 2700;
        Player.Y = 2300;
        Player.Z = 10;
        Player.ImageName = "Player.png";

        Player.SetPattern(83, 108);
        Player.GetFrameCount();
        Player.DoCenter = true;

        //create monster
        string[] Names = new[] { "DarkLord", "DesertWing", "EnragedFallen", "EnragedShaman", "HellBuzzard", "MegaDemon", "NightClan" };

        for (int i = 0; i < 1200; i++)
        {
            var Monster = new Monster(Game.SpriteEngine);
            var name = Names[Random.Next(0, 7)];
            Monster.Name = name;
            Monster.ImageName = name + ".png";
            Monster.X = Random.Next(0, 6000);
            Monster.Y = Random.Next(0, 6400);
            Monster.Z = 1;
         
            switch (Monster.ImageName)
            {
                case "DarkLord.png": Monster.SetPattern(54, 79); break;
                case "DesertWing.png": Monster.SetPattern(98, 77); break;
                case "HellBuzzard.png": Monster.SetPattern(70, 86); break;
                case "MegaDemon.png": Monster.SetPattern(171, 153); break;
                case "EnragedFallen.png": Monster.SetPattern(54, 64); break;
                case "EnragedShaman.png": Monster.SetPattern(123, 111); break;
                case "NightClan.png": Monster.SetPattern(46, 75); break;
            }
            Monster.GetFrameCount();
          
        }

        var Finger = new Finger(Game.SpriteEngine);
        Finger.ImageName = "Finger.png";
    }

}