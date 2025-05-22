using Afterwarp.SpriteEngine;
using Afterwarp.SpriteEngine;

namespace Z_Order;

public class Sprite1 : Sprite
{
    public Sprite1(Sprite Parent) : base(Parent)
    {
        X = 120;
        Y = 500;
    }
    public float Speed = 1.5f;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Y += Speed*Delta;
        if (Y < 10 || Y > 600)
            Speed = -Speed;
        //dynamic change Z
        Z = (int)Y - 50;
    }
}
public class Sprite2 : Sprite
{
    public Sprite2(Sprite Parent) : base(Parent)
    {
    }
    public float Speed = 0.8f;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Y += Speed*Delta;
        if (Y < 250 || Y > 370)
            Speed = -Speed;
        //dynamic change Z
        Z = (int)Y - 50;
    }
}

public class Sprite3 : AnimatedSprite
{
    public Sprite3(Sprite Parent) : base(Parent)
    {
      
    }
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        OnTimer(1500,()=>
        {
            Z=-Z;
        });  
    }
}
public class Sprites
{
    public static void Create()
    {
        for (int i = 0; i < 4; i++)
        {
            var Sprite = new SpriteEx(Game.SpriteEngine, "Tree6.png", 100, 50 + i * 150);
            Sprite.Z = (int)Sprite.Y;
        }
        Sprite1 Sprite1 = new Sprite1(Game.SpriteEngine);
        
        Sprite1.ImageName = "Dog.png";
        //
        for (int i = 1; i <= 2; i++)
        {
            var Sprite = new Sprite2(Game.SpriteEngine);
            Sprite.DoMove(1);
            if (i == 1)
            {
                Sprite.X = 400;
                Sprite.Y = 255;
                Sprite.ImageName = "Dog.png";
            }
            else
            {
                Sprite.X = 410;
                Sprite.Y = 369;
                Sprite.ImageName = "Cat.png";
            }
            Sprite.Z = (int)Sprite.Y;
        }
        //
        for (int i = 0; i < 10; i++)
        {
            var Sprite = new Sprite3(Game.SpriteEngine);
            Sprite.Move(1);
            Sprite.Init("NumberSet.png", 650 + i * 25, 100 + i * 45, i, 88, 88);
            Sprite.PatternIndex = 9 - i;

        }
    }

}
