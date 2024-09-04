using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ScreenSizeAdapter : MonoBehaviour
{
    public int referenceWidth = 1080;

    private RectTransform rect;
    private int lastScreenWidth = 1080;

    void Awake()
    {
        AdjustScale();
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth)
        {
            AdjustScale();
        }
    }

    void AdjustScale()
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }

        if (rect != null)
        {
            float currentWidth = Screen.width;
            float scale = currentWidth / referenceWidth;
            rect.localScale = Vector3.one*scale;

            lastScreenWidth = (int)currentWidth;
        }
    }

    void OnValidate()
    {
        AdjustScale();
    }
}
