using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static InputManager;

public class InputButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    public Key key;

    public void OnPointerDown(PointerEventData eventData)
    {
        down=true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        down = false;
    }

    public void Awake()
    {
        SetKeyEvent += SetKey;
    }

    public (Key, bool)[] SetKey()
    {
        return new (Key, bool)[] { (key, down) };
    }

    private bool down=false;
}
