using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Block : MonoBehaviour
{
    public int ID { get; protected set; } = 1;

    public Vector2Int Pos { get; private set; }= new Vector2Int();

    public virtual void Break()
    {
        Destroy(gameObject);
    }
    public virtual bool MoveTo(Vector2Int pos, float waitingTime, float movingTime)
    {
        if (nowTime > 0)
        {
            Debug.LogError("Already Moving!");
            return false;
        }
        moveStartPos = Pos;
        moveEndPos = pos;
        Pos = pos;
        this.waitingTime = waitingTime;
        this.movingTime = movingTime;
        nowTime = 0;
        ground.UpdateCall += Moving;
        return true;
    }

    public virtual void Init(Vector2Int pos,Ground ground)
    {
        Pos = pos;
        this.ground = ground;
        transform.localPosition = Frame.instance.GetPosV2(pos);
    }

    protected Ground ground;


    protected Vector2Int moveStartPos, moveEndPos;
    protected float waitingTime, movingTime,nowTime;
    protected virtual void Moving()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= waitingTime)
        {
            if (nowTime >= waitingTime+movingTime)
            {
                transform.localPosition = Frame.instance.GetPosV2(moveEndPos);
                ground.SetBlock(moveStartPos, 0);
                ground.SetBlock(moveEndPos, 0);
                ground.Blocks[moveEndPos.x, moveEndPos.y] = this;
                nowTime = 0;

                ground.UpdateCall -= Moving;
            }
            else
            {
                Vector2 start = Frame.instance.GetPosV2(moveEndPos);
                Vector2 end = Frame.instance.GetPosV2(moveEndPos);
                float t = (nowTime-waitingTime)/movingTime;
                t = Mathf.SmoothStep(0, 1, t);
                transform.localPosition = Vector3.Lerp(start, end, t);
            }
        }
    }
}
