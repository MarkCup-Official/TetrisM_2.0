using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Scene inputScene, levelScene, menuScene;
    public bool IsSceneLoading { get; private set; } = false;

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

    public void LoadLevel(int level)
    {
        if (IsSceneLoading)
        {
            Debug.LogWarning($"Already Loading scene");
            return;
        }
        IsSceneLoading = true;
        LoadSceneAdditivelyAsync($"Level_{level}", (Scene loadedScene) =>
        {
            levelScene = loadedScene;
            IsSceneLoading = false;
        });
    }

    public void LoadMenu(int menu)
    {
        if (IsSceneLoading)
        {
            Debug.LogWarning($"Already Loading scene");
            return;
        }
        IsSceneLoading = true;
        LoadSceneAdditivelyAsync($"Menu_{menu}", (Scene loadedScene) =>
        {
            menuScene = loadedScene;
            IsSceneLoading = false;
        });
    }

    private void LoadSceneAdditivelyAsync(string sceneName, Action<Scene> onSceneLoaded)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.completed += (AsyncOperation op) =>
        {
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);

            if (loadedScene.IsValid())
            {
                onSceneLoaded?.Invoke(loadedScene);
            }
            else
            {
                Debug.LogError($"Scene {sceneName} failed to load.");
            }
        };
    }
}
