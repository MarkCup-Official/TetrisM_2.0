using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Using call back function to get input value, this will prevent the delay of input
/// To set input you should use adapter to respond an input quest in SetKeyEvent
/// 
///  <summary>
/// Every input be get in this project should use this input manager, and this class could be only called on main thread
/// </summary>
public class InputManager
{
    public enum Key
    {
        left,
        right,
        up,
        down,

        a,
        b,
    }

    public static void Init()
    {
        if (inited)
        {
            return;
        }
        instance = new InputManager();
        inited = true;
    }

    public static bool GetKey(Key key)
    {
        CheckAndInit();
        instance.GetInput();
        return instance.isHold[key];
    }
    public static bool GetKeyDown(Key key)
    {
        CheckAndInit();
        instance.GetInput();
        return instance.isDown[key];
    }
    public static bool GetKeyUp(Key key)
    {
        CheckAndInit();
        instance.GetInput();
        return instance.isUp[key];
    }

    public delegate (Key, bool)[] SetKey();
    public static event SetKey SetKeyEvent;

    private static InputManager instance = null;

    private static bool inited = false;

    private Dictionary<Key, bool> isHold = new Dictionary<Key, bool>();
    private Dictionary<Key, bool> isDown = new Dictionary<Key, bool>();
    private Dictionary<Key, bool> isUp = new Dictionary<Key, bool>();

    private Dictionary<Key, bool> lastFrameRaw = new Dictionary<Key, bool>();
    private Dictionary<Key, bool> raw = new Dictionary<Key, bool>();

    private int updatedFrame;

    private static void CheckAndInit()
    {
        if (!inited)
        {
            Init();
        }
    }

    private InputManager()
    {
        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            isHold[key] = false;
            isDown[key] = false;
            isUp[key] = false;
            lastFrameRaw[key] = false;
            raw[key] = false;
        }
        updatedFrame = -1;
    }

    private void GetInput()
    {
        int nowFrame = Time.frameCount;
        if (updatedFrame != nowFrame)
        {
            updatedFrame = nowFrame;
            List<(Key, bool)> rawInput = InvokeInputeEvents();
            foreach ((Key key, bool hold) inp in rawInput)
            {
                raw[inp.key] = inp.hold;
            }
            foreach (KeyValuePair<Key, bool> pair in raw)
            {
                if (pair.Value)
                {
                    isHold[pair.Key] = true;
                }
                else
                {
                    isHold[pair.Key] = false;
                }
                if (!lastFrameRaw[pair.Key] && pair.Value)
                {
                    isDown[pair.Key] = true;
                }
                else
                {
                    isDown[pair.Key] = false;
                }
                if (lastFrameRaw[pair.Key] && !pair.Value)
                {
                    isUp[pair.Key] = true;
                }
                else
                {
                    isUp[pair.Key] = false;
                }
                lastFrameRaw[pair.Key]=pair.Value;
            }
        }
    }

    private List<(Key, bool)> InvokeInputeEvents()
    {
        List<(Key, bool)> res = new List<(Key, bool)>();
        if (SetKeyEvent != null)
        {
            foreach (SetKey handler in SetKeyEvent.GetInvocationList())
            {
                try
                {
                    (Key, bool)[] handlerResults = handler();
                    if (handlerResults != null)
                    {
                        res.AddRange(handlerResults);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Handler threw an exception: {ex.Message}");
                }
            }
        }
        return res;
    }
}
