using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InputManager;

public class MoveUI : MonoBehaviour
{
    public Image UI;

    public Sprite up,down,left,right,no;

    private void Update()
    {
        if (GetKey(Key.left) && !GetKey(Key.right))
        {
            UI.sprite = left;
        }
        else if (!GetKey(Key.left) && GetKey(Key.right))
        {
            UI.sprite = right;
        }
        else if (GetKey(Key.up)&&!GetKey(Key.down))
        {
            UI.sprite = up;
        }
        else if (!GetKey(Key.up) && GetKey(Key.down))
        {
            UI.sprite=down;
        }
        else
        {
            UI.sprite = no;
        }
    }
}
