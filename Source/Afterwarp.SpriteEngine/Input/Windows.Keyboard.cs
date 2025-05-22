
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace Afterwarp.SpriteEngine;

public static class _Keyboard
{
    private static readonly byte[] DefinedKeyCodes;

    private static readonly byte[] _keyState;

    private static readonly List<Keys> _keys;

    private static bool _isActive;

    private static readonly Predicate<Keys> IsKeyReleasedPredicate;

    public static KeyboardState GetState()
    {
        return PlatformGetState();
    }

    [Obsolete("Use GetState() instead. In future versions this method can be removed.")]
  //  public static KeyboardState GetState(PlayerIndex playerIndex)
   // {
      //  return PlatformGetState();
   // }

    [DllImport("user32.dll")]
    private static extern bool GetKeyboardState(byte[] lpKeyState);

    static _Keyboard()
    {
        _keyState = new byte[256];
        _keys = new List<Keys>(10);
        IsKeyReleasedPredicate = (Keys key) => IsKeyReleased((byte)key);
        Array values = Enum.GetValues(typeof(Keys));
        List<byte> list = new List<byte>(Math.Min(values.Length, 255));
        foreach (int item in values)
        {
            if (item >= 1 && item <= 255)
            {
                list.Add((byte)item);
            }
        }

        DefinedKeyCodes = list.ToArray();
    }

    private static KeyboardState PlatformGetState()
    {
        if ( GetKeyboardState(_keyState))
        {
            _keys.RemoveAll(IsKeyReleasedPredicate);
            byte[] definedKeyCodes = DefinedKeyCodes;
            foreach (byte b in definedKeyCodes)
            {
                if (!IsKeyReleased(b))
                {
                    Keys item = (Keys)b;
                    if (!_keys.Contains(item))
                    {
                        _keys.Add(item);
                    }
                }
            }
        }

        return new KeyboardState(_keys, Console.CapsLock, Console.NumberLock);
    }

    private static bool IsKeyReleased(byte keyCode)
    {
        return (_keyState[keyCode] & 0x80) == 0;
    }

    internal static void SetActive(bool isActive)
    {
        _isActive = isActive;
        if (!_isActive)
        {
            _keys.Clear();
        }
    }
}
