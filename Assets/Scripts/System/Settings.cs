using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Settings : MonoBehaviour
{
    public InputField inputField;

    public delegate void SettingChange(Dictionary<string, int> values);
    public static event SettingChange settingChange;

    public string savePath = "main";
    public string key;

    public static Dictionary<string,int> values;


    public void GetInputField()
    {
        if (!values.ContainsKey(key))
        {
            Debug.LogError("error key!");
            return;
        }
        bool success= int.TryParse(inputField.text, out int value);
        if (success)
        {
            values[key] = value;
        }
        else
        {
            Debug.LogError("error value!");
            return;
        }
        Save(values);
    }

    public void SetValue(int value)
    {
        if (!values.ContainsKey(key))
        {
            Debug.LogError("error key!");
            return;
        }
        values[key] = value;
        Save(values);
    }

    public void ResetSetting()
    {
        values = GetDefault();
    }
    private void SettingChangeF(Dictionary<string, int> values)
    {
        TryGetComponent(out IGetSetting getSetting);
        getSetting?.GetSetting(values[key]);
        if (inputField != null)
        {
            inputField.text = values[key].ToString();
        }
    }

    private void Save(Dictionary<string, int> values)
    {
        settingChange?.Invoke(values);
        string content = "";
        foreach(KeyValuePair<string, int> pair in values)
        {
            content += $"{pair.Key}:{pair.Value}\n";
        }
        IOManager.WriteToFile($"settings/{savePath}", content);
    }

    private Dictionary<string, int> ReadORInit()
    {
        string input= IOManager.ReadFromFile($"settings/{savePath}");

        if (input == null)
        {
            return GetDefault();
        }
        else
        {
            return Parse(input);
        }
    }

    private Dictionary<string, int> GetDefault()
    {
        Dictionary<string, int> result = new Dictionary<string, int>
        {
            { "left", (int)KeyCode.LeftArrow },
            { "right", (int)KeyCode.RightArrow },
            { "down", (int)KeyCode.DownArrow },
            { "downToBottom", (int)KeyCode.Space },
            { "leftRotate", (int)KeyCode.Z },
            { "rightRotate", (int)KeyCode.X },
            { "doubleRotate", (int)KeyCode.A },
            { "hold", (int)KeyCode.C},
            { "DAS",150 },
            { "ARR", 10 },
            { "ARRDown", 20},
        };

        Save(result);

        return result;
    }

    private Dictionary<string, int> Parse(string input)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        string[] lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                if (int.TryParse(parts[1].Trim(), out int value))
                {
                    result.Add(key, value);
                }
                else
                {
                    Debug.LogWarning($"Value is not an integer: {parts[1]}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid line format: {line}");
            }
        }

        return result;
    }

    private void Awake()
    {
        if (values == null)
        {
            values = ReadORInit();
        }
        TryGetComponent(out IGetSetting getSetting);
        getSetting?.GetSetting(values[key]);
        if (inputField != null)
        {
            inputField.text = values[key].ToString();
        }
        settingChange += SettingChangeF;
    }
    private void OnDestroy()
    {
        settingChange -= SettingChangeF;
    }
}

public interface IGetSetting
{
    void GetSetting(int value);
}