using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static Ground Instance { get; private set; }

    public Block[,] Blocks { get; private set; } = new Block[10, 20];
    public GameObject[] blockPrefabs;

    public delegate void Call();
    public Call UpdateCall;
    public delegate void Clear(int count);
    public Clear ClearCall;

    private void Awake()
    {
        Instance = this;
        Blocks = new Block[Frame.xSize, Frame.ySize];
    }

    private float timmer = 0;
    private const float tickTime= 0.1f;

    private void Update()
    {
        UpdateCall?.Invoke();

        timmer += Time.deltaTime;
        while (timmer >= tickTime)
        {
            timmer -= tickTime;
            DoTick();
            CheckFull();
        }
    }

    private void DoTick()
    {

    }

    private bool CheckFull()
    {
        List<int> full=new();
        for(int y=0; y<Frame.ySize; y++)
        {
            bool isFull = true;
            for (int x = 0; x < Frame.xSize; x++)
            {
                if (Blocks[x,y]==null|| Blocks[x, y].ID == 36 || Blocks[x, y].ID <= 0)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                full.Add(y);
            }
        }

        if (full.Count == 0)
        {
            return false;
        }


        int fall = 0;
        int[] falls= new int[Frame.ySize];
        int i = 0;
        for (int y = 0; y < Frame.ySize; y++)
        {
            if(full.Count>i&& full[i] == y)
            {
                for (int x = 0; x < Frame.xSize; x++)
                {
                    SetBlock(new Vector2Int(x, full[i]), 0);
                }
                fall ++;
                i++;
            }
            else
            {
                if (fall == 0)
                {
                    continue;
                }
                for (int x = 0; x < Frame.xSize; x++)
                {
                    MoveDown(new Vector2Int(x, y), fall);
                }
            }
        }

        ClearCall?.Invoke(full.Count);
        return true;
    }

    private bool MoveDown(Vector2Int start, int length)
    {
        if (!inRange(start) || !inRange(start+Vector2Int.down*length))
        {
            Debug.LogError("Position is out of range!");
            return false;
        }
        if(Blocks[start.x, start.y] == null)
        {
            return false;
        }
        bool couldMove = true;
        for(int i = 1; i <= length; i++)
        {
            Vector2Int end = start + Vector2Int.down * i;
            if (Blocks[end.x, end.y] == null || Blocks[end.x, end.y].ID <= 0 || Blocks[end.x, end.y].ID == 36)
            {
                continue;
            }
            else
            {
                couldMove = false;
                break;
            }
        }
        if (!couldMove)
        {
            Debug.LogWarning($"{start.x},{start.y}:Moving Failed!");
            return false;
        }
        bool moved = Blocks[start.x, start.y].MoveTo(start + Vector2Int.down * length, 0.1f, 0.2f);
        if (moved)
        {
            Blocks[start.x, start.y] = null;
            SetBlock(start, 36);//moving block
            SetBlock(start + Vector2Int.down * length, 36);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool inRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < Frame.xSize && pos.y < Frame.ySize;
    }

    public int GetBlockID(Vector2Int pos)
    {
        if(pos.y >= Frame.ySize&& pos.x >= 0 && pos.x < Frame.xSize)
        {
            return 0;
        }
        if (pos.x < 0 || pos.y < 0 || pos.x >= Frame.xSize || pos.y >= Frame.ySize)
        {
            return 1;
        }
        if(Blocks[pos.x, pos.y] != null)
        {
            return Blocks[pos.x, pos.y].ID;
        }
        else
        {
            return 0;
        }
    }

    public void SetBlock(Vector2Int pos,int id)
    {
        if (id == 0)
        {
            if (Blocks[pos.x, pos.y] != null)
            {
                Blocks[pos.x, pos.y].Break();
                Blocks[pos.x, pos.y]=null;
            }
            return;
        }
        if(id<0||id> blockPrefabs.Length)
        {
            Debug.LogError("Index is out of range!");
            return;
        }
        if (pos.x < 0 || pos.y < 0 || pos.x >= Frame.xSize || pos.y >= Frame.ySize)
        {
            Debug.LogWarning("Pos is out of range!");
            return;
        }
        Blocks[pos.x, pos.y] = Instantiate(blockPrefabs[id]).GetComponent<Block>();
        Blocks[pos.x, pos.y].transform.SetParent(transform, false);
        Blocks[pos.x, pos.y].Init(new Vector2Int(pos.x, pos.y),this);
    }
}
