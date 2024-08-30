using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Update()
    {
        if (InputManager.GetKeyDown(InputManager.Key.a))
        {
            Debug.Log(1);
        }
        if (InputManager.GetKey(InputManager.Key.a))
        {
            Debug.Log(2);
        }
        if (InputManager.GetKeyUp(InputManager.Key.a))
        {
            Debug.Log(3);
        }
    }
}
