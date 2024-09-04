using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IOManager
{
    public static string ReadFromFile(string path)
    {
        path=Path.Combine(Application.persistentDataPath, path);
        try
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                return content;
            }
            else
            {
                Debug.LogWarning("File not found: " + path);
                return null;
            }
        }
        catch (IOException e)
        {
            Debug.LogError("An error occurred while reading the file: " + e.Message);
            return null;
        }
    }

    public static void WriteToFile(string path, string content)
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log(path);
        path = Path.Combine(Application.persistentDataPath, path);

        Debug.Log(path);
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        try
        {
            File.WriteAllText(path, content);
            Debug.Log($"File {path} written successfully");
        }
        catch (IOException e)
        {
            Debug.LogError("An error occurred while writing to the file: " + e.Message);
        }
    }
}
