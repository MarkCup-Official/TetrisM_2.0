using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadSceneButton : MonoBehaviour,IPointerClickHandler
{
    public string SceneName;
    public bool load;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (load)
        {
            GameManager.instance.LoadSceneName(SceneName);
        }
        else
        {
            GameManager.instance.CloseSceneName(SceneName);
        }
    }
}
