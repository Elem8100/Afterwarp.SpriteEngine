using System.Numerics;
using System.Reflection;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Afterwarp.SpriteEngine;

public static class DictionaryExtension
{
    public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        dict[key] = value;
        return dict;
    }
}

public class Game
{
    public static Dictionary<string, Texture> TextureLib = new();
    public static Device Device;
    public static Canvas Canvas;
    public static TextRenderer TextRenderer;
    public static Afterwarp.Timer Timer;
    public static SwapChain SwapChain;
    public static SpriteEngine SpriteEngine = new SpriteEngine(null);
    public static SpriteEngine BackgroundEngine = new SpriteEngine(null);
    public static Form Form;
    static Vector2 DisplaySize;

    public static void Init(int MultiSamples = 4, bool Vsync = false)
    {
        Form = System.Windows.Forms.Application.OpenForms[0];
        try
        {
            Device = new Device(DeviceTechnology.Direct3D);
            SwapChain = new SwapChain(Device, Form.Handle, null, PixelFormat.Unknown, PixelFormat.Unknown, MultiSamples, Vsync);
            Canvas = new Canvas(Device);
            TextRenderer = new TextRenderer(Canvas, new Point(512, 512), PixelFormat.LA8);
            Timer = new Timer();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Initialization failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Windows.Forms.Application.Exit();
        }

        typeof(Form).InvokeMember("SetStyle",
         BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null,
         Form, new object[] { ControlStyles.Opaque, true });

        typeof(Form).InvokeMember("SetStyle",
         BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null,
         Form, new object[] { ControlStyles.UserPaint, true });

        typeof(Form).InvokeMember("SetStyle",
        BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null,
        Form, new object[] { ControlStyles.AllPaintingInWmPaint, true });

        typeof(Form).InvokeMember("SetStyle",
         BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null,
         Form, new object[] { ControlStyles.DoubleBuffer, false });

        System.Windows.Forms.Application.Idle += (s, e) => { Form.Invalidate(); };
        Form.Resize += (s, e) =>
        {
            if (Form.WindowState != FormWindowState.Minimized && Game.Device is not null)
            {
                DisplaySize = new Vector2(Form.ClientSize.Width, Form.ClientSize.Height);
                if (Game.SwapChain != null)
                    Game.SwapChain.Resize(new Point(DisplaySize));
                Form.Invalidate();
            }
        };
      
        MouseEx.Init();
    }
    public static void LoadTextrues(string Dir)
    {
        DirectoryInfo Folder = new DirectoryInfo(Dir);
        foreach (FileInfo File in Folder.GetFiles())
        {
            if (File.Extension == ".png" || File.Extension == ".jpg")
            {
                var Texture = new Texture(Device, File.FullName, PixelFormat.Unknown, 16);
                TextureLib.Add(File.Name, Texture);
            }
        }
    }
    public static void Dispose()
    {
        Canvas?.Dispose();
        SwapChain?.Dispose();
        Device?.Dispose();
    }

    public static void Draw(uint ClearColor)
    {
        if (Form.WindowState == FormWindowState.Minimized)
            return;
      
        SwapChain.Begin();
        try
        {
            Device.Clear(DeviceClear.Color, new FloatColor(ClearColor));
            Canvas.Begin();
            try
            {
                Game.Canvas.ContextState = CanvasContextState.FlatScene;
                SpriteEngine.Draw();
                SpriteEngine.Move(Timer.Latency * 0.00006f);
                SpriteEngine.Dead();
            }
            finally
            {
                Canvas.End();
            }
        }
        finally
        {
            SwapChain.End();
        }
       // Timer.NextSlice();
        Timer.Update();
    }

    public static void Draw(uint ClearColor, Action Action)
    {
        if (Form.WindowState == FormWindowState.Minimized)
            return;
        SwapChain.Begin();
        try
        {
            Device.Clear(DeviceClear.Color, new FloatColor(ClearColor));
            Canvas.Begin();
            try
            {
                Game.Canvas.ContextState = CanvasContextState.FlatScene;
                Action();
            }
            finally
            {
                Canvas.End();
            }
        }
        finally
        {
            SwapChain.End();
        }
        Timer.Update();
    }


}