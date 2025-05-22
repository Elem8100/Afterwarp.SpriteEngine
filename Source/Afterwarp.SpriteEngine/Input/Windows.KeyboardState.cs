using System;
using System.Collections.Generic;

namespace Afterwarp.SpriteEngine;

public struct KeyboardState
{
    private const byte CapsLockModifier = 1;

    private const byte NumLockModifier = 2;

    private static Keys[] empty = new Keys[0];

    private uint _keys0;

    private uint _keys1;

    private uint _keys2;

    private uint _keys3;

    private uint _keys4;

    private uint _keys5;

    private uint _keys6;

    private uint _keys7;

    private byte _modifiers;

    public bool CapsLock => (_modifiers & 1) > 0;

    public bool NumLock => (_modifiers & 2) > 0;

    public KeyState this[Keys key]
    {
        get
        {
            if (!InternalGetKey(key))
            {
                return KeyState.Up;
            }

            return KeyState.Down;
        }
    }

    private bool InternalGetKey(Keys key)
    {
        uint num = (uint)(1 << (int)(key & (Keys)31));
        return ((uint)(((int)key >> 5) switch
        {
            0 => (int)_keys0,
            1 => (int)_keys1,
            2 => (int)_keys2,
            3 => (int)_keys3,
            4 => (int)_keys4,
            5 => (int)_keys5,
            6 => (int)_keys6,
            7 => (int)_keys7,
            _ => 0,
        }) & num) != 0;
    }

    internal void InternalSetKey(Keys key)
    {
        uint num = (uint)(1 << (int)(key & (Keys)31));
        switch ((int)key >> 5)
        {
            case 0:
                _keys0 |= num;
                break;
            case 1:
                _keys1 |= num;
                break;
            case 2:
                _keys2 |= num;
                break;
            case 3:
                _keys3 |= num;
                break;
            case 4:
                _keys4 |= num;
                break;
            case 5:
                _keys5 |= num;
                break;
            case 6:
                _keys6 |= num;
                break;
            case 7:
                _keys7 |= num;
                break;
        }
    }

    internal void InternalClearKey(Keys key)
    {
        uint num = (uint)(1 << (int)(key & (Keys)31));
        switch ((int)key >> 5)
        {
            case 0:
                _keys0 &= ~num;
                break;
            case 1:
                _keys1 &= ~num;
                break;
            case 2:
                _keys2 &= ~num;
                break;
            case 3:
                _keys3 &= ~num;
                break;
            case 4:
                _keys4 &= ~num;
                break;
            case 5:
                _keys5 &= ~num;
                break;
            case 6:
                _keys6 &= ~num;
                break;
            case 7:
                _keys7 &= ~num;
                break;
        }
    }

    internal void InternalClearAllKeys()
    {
        _keys0 = 0u;
        _keys1 = 0u;
        _keys2 = 0u;
        _keys3 = 0u;
        _keys4 = 0u;
        _keys5 = 0u;
        _keys6 = 0u;
        _keys7 = 0u;
    }

    internal KeyboardState(List<Keys> keys, bool capsLock = false, bool numLock = false)
    {
        this = default(KeyboardState);
        _keys0 = 0u;
        _keys1 = 0u;
        _keys2 = 0u;
        _keys3 = 0u;
        _keys4 = 0u;
        _keys5 = 0u;
        _keys6 = 0u;
        _keys7 = 0u;
        _modifiers = (byte)(0u | (capsLock ? 1u : 0u) | (numLock ? 2u : 0u));
        if (keys == null)
        {
            return;
        }

        foreach (Keys key in keys)
        {
            InternalSetKey(key);
        }
    }

    public KeyboardState(Keys[] keys, bool capsLock = false, bool numLock = false)
    {
        this = default(KeyboardState);
        _keys0 = 0u;
        _keys1 = 0u;
        _keys2 = 0u;
        _keys3 = 0u;
        _keys4 = 0u;
        _keys5 = 0u;
        _keys6 = 0u;
        _keys7 = 0u;
        _modifiers = (byte)(0u | (capsLock ? 1u : 0u) | (numLock ? 2u : 0u));
        if (keys != null)
        {
            foreach (Keys key in keys)
            {
                InternalSetKey(key);
            }
        }
    }

    public KeyboardState(params Keys[] keys)
    {
        this = default(KeyboardState);
        _keys0 = 0u;
        _keys1 = 0u;
        _keys2 = 0u;
        _keys3 = 0u;
        _keys4 = 0u;
        _keys5 = 0u;
        _keys6 = 0u;
        _keys7 = 0u;
        _modifiers = 0;
        if (keys != null)
        {
            foreach (Keys key in keys)
            {
                InternalSetKey(key);
            }
        }
    }

    public bool IsKeyDown(Keys key)
    {
        return InternalGetKey(key);
    }

    public bool IsKeyUp(Keys key)
    {
        return !InternalGetKey(key);
    }

    public int GetPressedKeyCount()
    {
        return (int)(CountBits(_keys0) + CountBits(_keys1) + CountBits(_keys2) + CountBits(_keys3) + CountBits(_keys4) + CountBits(_keys5) + CountBits(_keys6) + CountBits(_keys7));
    }

    private static uint CountBits(uint v)
    {
        v -= (v >> 1) & 0x55555555;
        v = (v & 0x33333333) + ((v >> 2) & 0x33333333);
        return ((v + (v >> 4)) & 0xF0F0F0F) * 16843009 >> 24;
    }

    private static int AddKeysToArray(uint keys, int offset, Keys[] pressedKeys, int index)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((keys & (1 << i)) != 0L)
            {
                pressedKeys[index++] = (Keys)(offset + i);
            }
        }

        return index;
    }

    public Keys[] GetPressedKeys()
    {
        uint num = CountBits(_keys0) + CountBits(_keys1) + CountBits(_keys2) + CountBits(_keys3) + CountBits(_keys4) + CountBits(_keys5) + CountBits(_keys6) + CountBits(_keys7);
        if (num == 0)
        {
            return empty;
        }

        Keys[] array = new Keys[num];
        int index = 0;
        if (_keys0 != 0)
        {
            index = AddKeysToArray(_keys0, 0, array, index);
        }

        if (_keys1 != 0)
        {
            index = AddKeysToArray(_keys1, 32, array, index);
        }

        if (_keys2 != 0)
        {
            index = AddKeysToArray(_keys2, 64, array, index);
        }

        if (_keys3 != 0)
        {
            index = AddKeysToArray(_keys3, 96, array, index);
        }

        if (_keys4 != 0)
        {
            index = AddKeysToArray(_keys4, 128, array, index);
        }

        if (_keys5 != 0)
        {
            index = AddKeysToArray(_keys5, 160, array, index);
        }

        if (_keys6 != 0)
        {
            index = AddKeysToArray(_keys6, 192, array, index);
        }

        if (_keys7 != 0)
        {
            index = AddKeysToArray(_keys7, 224, array, index);
        }

        return array;
    }

    public void GetPressedKeys(Keys[] keys)
    {
        if (keys == null)
        {
            throw new ArgumentNullException("keys");
        }

        if (CountBits(_keys0) + CountBits(_keys1) + CountBits(_keys2) + CountBits(_keys3) + CountBits(_keys4) + CountBits(_keys5) + CountBits(_keys6) + CountBits(_keys7) > keys.Length)
        {
            throw new ArgumentOutOfRangeException("keys", "The supplied array cannot fit the number of pressed keys. Call GetPressedKeyCount() to get the number of pressed keys.");
        }

        int num = 0;
        if (_keys0 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys0, 0, keys, num);
        }

        if (_keys1 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys1, 32, keys, num);
        }

        if (_keys2 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys2, 64, keys, num);
        }

        if (_keys3 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys3, 96, keys, num);
        }

        if (_keys4 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys4, 128, keys, num);
        }

        if (_keys5 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys5, 160, keys, num);
        }

        if (_keys6 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys6, 192, keys, num);
        }

        if (_keys7 != 0 && num < keys.Length)
        {
            num = AddKeysToArray(_keys7, 224, keys, num);
        }
    }

    public override int GetHashCode()
    {
        return (int)(_keys0 ^ _keys1 ^ _keys2 ^ _keys3 ^ _keys4 ^ _keys5 ^ _keys6 ^ _keys7);
    }

    public static bool operator ==(KeyboardState a, KeyboardState b)
    {
        if (a._keys0 == b._keys0 && a._keys1 == b._keys1 && a._keys2 == b._keys2 && a._keys3 == b._keys3 && a._keys4 == b._keys4 && a._keys5 == b._keys5 && a._keys6 == b._keys6)
        {
            return a._keys7 == b._keys7;
        }

        return false;
    }

    public static bool operator !=(KeyboardState a, KeyboardState b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is KeyboardState)
        {
            return this == (KeyboardState)obj;
        }

        return false;
    }
}