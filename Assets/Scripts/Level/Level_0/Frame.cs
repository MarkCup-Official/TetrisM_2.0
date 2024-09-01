using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public static Frame instance { get; private set; }

    public static int xSize=10, ySize=20;

    public Transform[,] points=new Transform[10,20];

    public float blockSize=1;

    private const int size = 60,pixelSize=160;

    private void Awake()
    {
        instance = this;

        points = new Transform[xSize, ySize];

        blockSize = (float)pixelSize / size / xSize;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                points[x, y] = GeneratePoint(new Vector2(blockSize * (-xSize / 2f + x+0.5f), blockSize * (-ySize / 2f + y + 0.5f)), $"Point_{x}_{y}");
            }
        }
    }

    public Vector2 GetPosV2(int x,int y)
    {
        return new Vector2(blockSize * (-xSize / 2f + x + 0.5f), blockSize * (-ySize / 2f + y + 0.5f));
    }
    public Vector2 GetPosV2(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        return new Vector2(blockSize * (-xSize / 2f + x + 0.5f), blockSize * (-ySize / 2f + y + 0.5f));
    }

    private Transform GeneratePoint(Vector2 vector2,string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform, false);
        go.transform.localPosition = vector2;
        return go.transform;
    }
}
