using Afterwarp.SpriteEngine;
namespace Particle;
public class Sprites
{
    public static void CreateParticle()
    {
        Random Random = new Random();
        for (int i = 0; i <= 400; i++)
        {
            if (Random.Next(0, 8) == 0)
            {
                var Particle = new ParticleSprite(Game.SpriteEngine);
                Particle.Init("Fire.png", Random.Next(0, 1000), 680, 2, 128, 128, 128, 128);
                Particle.SetAnim("Fire.png", 0, 32, 0.3f, false, false, true);
                Particle.LifeTime = 180;
                Particle.Decay = 1f;
                Particle.UpdateSpeed = 1f;
                Particle.AccelX = 0.0f;
                Particle.AccelY = -(0.0025f + (Random.Next(0, 11) / 200)) * 10 * 0.017f;
                Particle.VelocityY = -(Random.Next(0, 21) / 4) * 80 * 0.017f;
                Particle.Angle = Random.Next(0, 628) * 0.01f;
            }
        }

    }

    public static void CreateScanline()
    {
        var Tile = new BackgroundSprite(Game.SpriteEngine);
        Tile.Init("Scanline.png", 0, 0, 5, 64, 64, 64, 64);
        Tile.TileMode = TileMode.Full;
        Tile.BlendingEffect = Afterwarp.BlendingEffect.Multiply;
    }

}

