using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager;
using UnityEngine.EventSystems;

public class KeyboardInput : MonoBehaviour
{
    public KeyCode 
        left=KeyCode.LeftArrow,
        right= KeyCode.RightArrow,
        down= KeyCode.DownArrow,
        down2Bottom= KeyCode.Space,
        leftSpin= KeyCode.Z,
        rightSpin= KeyCode.X,
        spin180= KeyCode.A,
        hold=KeyCode.C;

    public void Awake()
    {
        SetKeyEvent += SetKey;
    }

    public (Key, bool)[] SetKey()
    {
        return new (Key, bool)[] { (Key.a, Input.GetKey(down2Bottom)) , (Key.b, Input.GetKey(rightSpin)) , (Key.x, Input.GetKey(leftSpin)) , (Key.y, Input.GetKey(spin180)) , (Key.up, Input.GetKey(hold)) , (Key.down, Input.GetKey(down)) , (Key.left, Input.GetKey(left)) , (Key.right, Input.GetKey(right)) };
    }
}
