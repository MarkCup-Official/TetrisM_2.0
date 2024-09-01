using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FallingBlock;

public class FallingGroupShow : MonoBehaviour
{

    public GameObject[] Blocks { get; private set; }
    public GroupTypes Type { get; private set; }
    public Rotations Rotate { get; private set; }

    public void Show(GroupTypes type, Rotations rotation)
    {
        int[,,] dat = groupDic[type];
        for (int i = 0; i < 4; i++)
        {
            Blocks[i].transform.localPosition = new Vector3(dat[(int)rotation, i, 0] * Frame.instance.blockSize, dat[(int)rotation, i, 1] * Frame.instance.blockSize);
        }
    }

    public void SetType(GroupTypes t)
    {
        Type = t;
        Show(Type, Rotate);
    }

    public void SetRotation(Rotations r)
    {
        Rotate = r;
        Show(Type, Rotate);
    }

    public void RotateTo(Rotations r)
    {
        Rotate = r;
        Show(Type, Rotate);
    }

    private void Awake()
    {
        Blocks = new GameObject[4];
        for(int i = 0;i < 4; i++)
        {
            Blocks[i]=transform.GetChild(i).gameObject;
        }
    }
}
