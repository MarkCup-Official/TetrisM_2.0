using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySelect : MonoBehaviour,IPointerClickHandler,IGetSetting
{
    public Text text;

    public string activeText = "按键盘的任意按键", disableText = "点击此处修改";

    public static KeySelect nowSelect=null;

    private Settings settings;

    private Text selfText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (nowSelect != this)
        {
            if (nowSelect != null)
            {
                nowSelect.SetActive(false);
                nowSelect = this;
            }
        }
        nowSelect = this;
        SetActive(true);
    }

    private bool isGettingKey = false;

    public void SetActive(bool b)
    {
        isGettingKey = b;
        if (b)
        {
            if (!selfText)
            {
                selfText = GetComponent<Text>();
            }
            selfText.text = activeText;
        }
        else
        {
            if (!selfText)
            {
                selfText = GetComponent<Text>();
            }
            selfText.text = disableText;
        }
    }
    private void Awake()
    {
        SetActive(false);
    }

    private void Update()
    {
        if (!isGettingKey)
        {
            return;
        }
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keyCode == KeyCode.Mouse0)
            {
                continue;
            }
            if (Input.GetKeyDown(keyCode))
            {
                nowSelect = null;
                SetActive(false);
                if (settings == null)
                {
                    settings= GetComponent<Settings>();
                }
                settings.SetValue((int)keyCode);
                text.text=Enum.GetName(typeof(KeyCode), keyCode);
                break;
            }
        }
    }

    public void GetSetting(int value)
    {
        try
        {
            KeyCode code = (KeyCode)value;
            text.text = Enum.GetName(typeof(KeyCode), code);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
