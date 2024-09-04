using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> loadedScene = new();
    public List<string> loadingScene = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There should not be two GameManagers");
            Destroy(gameObject);
        }

        //LoadSceneAdditivelyAsync("Input", (Scene loadedScene) =>{inputScene = loadedScene;});
        //LoadLevel(0);
    }

    public void LoadSceneName(string name)
    {
        if (loadedScene.Contains(name))
        {
            Debug.LogWarning($"Scene already exist");
            return;
        }
        if (loadingScene.Contains(name))
        {
            Debug.LogWarning($"Loading scene");
            return;
        }
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            LoadScene(name);
        }
        else
        {
            Debug.LogError($"{name} is not in build scenes!");
        }
    }
    public void CloseSceneName(string name)
    {
        if (!loadedScene.Contains(name))
        {
            Debug.LogWarning($"Scene not exist");
            return;
        }
        if (loadingScene.Contains(name))
        {
            Debug.LogWarning($"Loading scene");
            return;
        }
        UnloadScene(name);
    }

    private void LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadingScene.Add(sceneName);
        asyncLoad.completed += (a) => { loadedScene.Add(sceneName); loadingScene.Remove(sceneName); };
    }
    private void UnloadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);
        loadingScene.Add(sceneName);
        asyncLoad.completed += (a) => { loadedScene.Remove(sceneName); loadingScene.Remove(sceneName); };
    }
}
