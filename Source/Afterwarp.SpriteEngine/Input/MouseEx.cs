namespace Afterwarp.SpriteEngine;
public class MouseEx
{
    public static void Init()
    {

        Game.Form.MouseDown += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftPressed = true;
                _LeftDown = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                RightPressed = true;
                _RightDown = true;
            }
        };

        Game.Form.MouseUp += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                _LeftDown = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                _RightDown = false;
            }
        };

        Game.Form.MouseMove += (s, e) =>
        {
            X = e.X;
            Y = e.Y;
        };

    }
    static bool LeftPressed, RightPressed;
    static bool _LeftDown, _RightDown;
    public static int X;
    public static int Y;
    public static bool LeftClick
    {
        get
        {
            bool LastDown = false;
            LastDown = LeftPressed;
            LeftPressed = false;
            return LastDown;
        }
    }
    public static bool RightClick
    {
        get
        {
            bool LastDown = false;
            LastDown = RightPressed;
            RightPressed = false;
            return LastDown;
        }
    }
    public static bool LeftDown => _LeftDown;
    public static bool RightDown => _RightDown;

}
