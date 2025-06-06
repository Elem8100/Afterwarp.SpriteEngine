﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Afterwarp.SpriteEngine;

public class Keyboard
{
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;

    public static KeyboardState GetState()
    {
        previousKeyState = currentKeyState;
        currentKeyState = _Keyboard.GetState();
        return currentKeyState;
    }

    public static bool KeyDown(Keys key)
    {
        return currentKeyState.IsKeyDown(key);
    }
    public static bool KeyUp(Keys key)
    {
        return !previousKeyState.IsKeyUp(key) && currentKeyState.IsKeyUp(key);
    }

    public static bool KeyPressed(Keys key)
    {
        return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
    }

}
/*
public class Mouse1
{
    static MouseState lastMouseState;
    static MouseState currentMouseState;

    public static MouseState GetState()
    {
        lastMouseState = currentMouseState;
        currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        return currentMouseState;

    }
    public static MouseState State
    {
        get => currentMouseState;
    }
    public static bool LeftClick
    {
        get => currentMouseState.LeftButton == ButtonState.Pressed &&
            lastMouseState.LeftButton == ButtonState.Released;

    }

    public static bool RightClick()
    {
        return currentMouseState.RightButton == ButtonState.Pressed &&
            lastMouseState.RightButton == ButtonState.Released;
    }
    public static bool RightPressed()
    {
        return currentMouseState.RightButton == ButtonState.Pressed;
    }
}
*/