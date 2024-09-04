using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static FallingGroupShow;
using static InputManager;
using static Unity.Burst.Intrinsics.X86.Avx;

public class FallingBlock : MonoBehaviour
{
    public enum GroupTypes
    {
        I=0,
        O=1,
        L=2,
        J=3,
        T=4,
        Z=5,
        ZM=6,
    }
    public enum Rotations
    {
        Zero = 0,
        R = 1,
        Two = 2,
        L = 3,
    }
    public static Dictionary<GroupTypes, float[]> cneterDic = new()
    {
    { GroupTypes.I,new float[]{ 2,2} } ,
    { GroupTypes.J,new float[]{ 1.5f,1.5f } } ,
    { GroupTypes.L,new float[]{ 1.5f,1.5f } } ,
    { GroupTypes.O,new float[]{ 2, 2 } } ,
    { GroupTypes.ZM,new float[]{ 1.5f, 1.5f } } ,
    { GroupTypes.T,new float[]{ 1.5f, 1.5f } } ,
    { GroupTypes.Z,new float[]{ 1.5f, 1.5f } } ,
    };
    public static Dictionary<GroupTypes, int[,,]> groupDic = new() {
        { GroupTypes.I,new int[,,]{
            {
        {0, 2},
        {1, 2},
        {2, 2},
        {3, 2},
    },
    {
        {2, 3},
        {2, 2},
        {2, 1},
        {2, 0},
    },
    {
        {3, 1},
        {2, 1},
        {1, 1},
        {0, 1},
    },
    {
        {1, 0},
        {1, 1},
        {1, 2},
        {1, 3},
    },} },
        { GroupTypes.J,new int[,,]{
            {
        {0, 2},
        {0, 1},
        {1, 1},
        {2, 1},
    },
    {
        {2, 2},
        {1, 2},
        {1, 1},
        {1, 0},
    },
    {
        {2, 0},
        {2, 1},
        {1, 1},
        {0, 1},
    },
    {
        {0, 0},
        {1, 0},
        {1, 1},
        {1, 2},
    },} },
        { GroupTypes.L,new int[,,]{
            {
        {0, 1},
        {1, 1},
        {2, 1},
        {2, 2},
    },
    {
        {1, 2},
        {1, 1},
        {1, 0},
        {2, 0},
    },
    {
        {2, 1},
        {1, 1},
        {0, 1},
        {0, 0},
    },
    {
        {1, 0},
        {1, 1},
        {1, 2},
        {0, 2},
    },} },
        { GroupTypes.O,new int[,,]{
            {
        {1, 1},
        {1, 2},
        {2, 2},
        {2, 1},
    },
            {
        {1, 2},
        {2, 2},
        {2, 1},
        {1, 1},
    },
            {
        {2, 2},
        {2, 1},
        {1, 1},
        {1, 2},
    },
            {
        {2, 1},
        {1, 1},
        {1, 2},
        {2, 2},
    },} },
        { GroupTypes.ZM,new int[,,]{
            {
        {0, 1},
        {1, 1},
        {1, 2},
        {2, 2},
    },
    {
        {1, 2},
        {1, 1},
        {2, 1},
        {2, 0},
    },
    {
        {2, 1},
        {1, 1},
        {1, 0},
        {0, 0},
    },
    {
        {1, 0},
        {1, 1},
        {0, 1},
        {0, 2},
    }, } },
        { GroupTypes.T,new int[,,]{
            {
        {0, 1},
        {1, 1},
        {2, 1},
        {1, 2},
    },
    {
        {1, 2},
        {1, 1},
        {1, 0},
        {2, 1},
    },
    {
        {2, 1},
        {1, 1},
        {0, 1},
        {1, 0},
    },
    {
        {1, 0},
        {1, 1},
        {1, 2},
        {0, 1},
    }, } },
        { GroupTypes.Z,new int[,,]{
            {
        {0, 2},
        {1, 2},
        {1, 1},
        {2, 1},
    },
    {
        {2, 2},
        {2, 1},
        {1, 1},
        {1, 0},
    },
    {
        {2, 0},
        {1, 0},
        {1, 1},
        {0, 1},
    },
    {
        {0, 0},
        {0, 1},
        {1, 1},
        {1, 2},
    }, } },
    };
    public static Dictionary<(Rotations,Rotations), int[,]> rotareCheck = new()
    {
        { (Rotations.Zero,Rotations.R),new int[,]
        {
            {0,0},
            {-1,0},
            {-1,1},
            {0,-2},
            {-1,-2},
        }
        },
        { (Rotations.R,Rotations.Zero),new int[,]
        {
            {0,0},
            {1,0},
            {1,-1},
            {0,2},
            {1,2},
        }
        },
        { (Rotations.R,Rotations.Two),new int[,]
        {
            {0,0},
            {1,0},
            {1,-1},
            {0,2},
            {1,2},
        }
        },
        { (Rotations.Two,Rotations.R),new int[,]
        {
            {0,0},
            {-1,0},
            {-1,1},
            {0,-2},
            {-1,-2},
        }
        },
        { (Rotations.Two,Rotations.L),new int[,]
        {
            {0,0},
            {1,0},
            {1,1},
            {0,-2},
            {1,-2},
        }
        },
        { (Rotations.L,Rotations.Two),new int[,]
        {
            {0,0},
            {-1,0},
            {-1,-1},
            {0,2},
            {-1,2},
        }
        },
        { (Rotations.L,Rotations.Zero),new int[,]
        {
            {0,0},
            {-1,0},
            {-1,-1},
            {0,2},
            {-1,2},
        }
        },
        { (Rotations.Zero,Rotations.L),new int[,]
        {
            {0,0},
            {1,0},
            {1,1},
            {0,-2},
            {1,-2},
        }
        },
    };
    public static Dictionary<(Rotations, Rotations), int[,]> rotareCheckI = new() 
    {
        { (Rotations.Zero,Rotations.R),new int[,]
        {
            {0,0},
            {-2,0},
            {1,0},
            {-2,-1},
            {1,2},
        }
        },
        { (Rotations.R,Rotations.Zero),new int[,]
        {
            {0,0},
            {2,0},
            {-1,0},
            {2,1},
            {-1,-2},
        }
        },
        { (Rotations.R,Rotations.Two),new int[,]
        {
            {0,0},
            {-1,0},
            {2,0},
            {-1,2},
            {2,-1},
        }
        },
        { (Rotations.Two,Rotations.R),new int[,]
        {
            {0,0},
            {1,0},
            {-2,0},
            {1,-2},
            {-2,1},
        }
        },
        { (Rotations.Two,Rotations.L),new int[,]
        {
            {0,0},
            {2,0},
            {-1,0},
            {2,1},
            {-1,-2},
        }
        },
        { (Rotations.L,Rotations.Two),new int[,]
        {
            {0,0},
            {-2,0},
            {1,0},
            {-2,-1},
            {1,2},
        }
        },
        { (Rotations.L,Rotations.Zero),new int[,]
        {
            {0,0},
            {1,0},
            {-2,0},
            {1,-2},
            {-2,1},
        }
        },
        { (Rotations.Zero,Rotations.L),new int[,]
        {
            {0,0},
            {-1,0},
            {2,0},
            {-1,2},
            {2,-1},
        }
        },
    };
    public struct FallingBlocks
    {
        public GroupTypes type;
        public int[] blockIDs;
    }

    public FallingGroupShow fallingGroupShow;
    public GroupTypes Type { get; private set; }
    public Rotations Rotation { get; private set; }
    public int[] BlockID { get; private set; }=new int[4] { 1,1,1,1};

    public Vector2Int Pos { get; private set; } = new(3, 21);

    public delegate void Call();
    public event Call OnReload;
    public event Call OnHold;

    public List<GroupTypes> waiting = new();
    public List<GroupTypes> bag = new();
    public GroupTypes Holding = GroupTypes.I;
    public bool isHolding = false;
    public bool couldHold = true;
    public int DAS=500, ARR=50,ARR_down=20;

    public void Reload()
    {
        couldHold = true;
        Pos =new Vector2Int(3, 18);
        timmer = fallingTime;
        Type = GetOneFromWaiting();
        Rotation = Rotations.Zero;

        transform.localPosition = Frame.instance.GetPosV2(Pos);
        fallingGroupShow.SetType(Type);
        fallingGroupShow.SetRotation(Rotation);

        OnReload?.Invoke();
    }

    private const float fallingTime = 1;
    private float timmer = fallingTime;
    private float downCold = 0;
    private float moveCold = 0;

    private GroupTypes GetOneFromWaiting()
    {
        while (waiting.Count < 5)
        {
            waiting.Add(GetOneFromBag());
        }
        GroupTypes res = waiting[0];
        waiting.RemoveAt(0);
        return res;
    }
    private GroupTypes GetOneFromBag()
    {
        if (bag.Count == 0)
        {
            bag = new() { GroupTypes.I, GroupTypes.O, GroupTypes.L, GroupTypes.J, GroupTypes.T, GroupTypes.Z, GroupTypes.ZM };
        }
        int i=Random.Range(0, bag.Count);
        GroupTypes res= bag[i];
        bag.RemoveAt(i);
        return res;
    }

    private void SettingChange(Dictionary<string, int> values)
    {
        DAS = values["DAS"];
        ARR = values["ARR"];
        ARR_down = values["ARRDown"];
    }

    private void Start()
    {
        Reload();
        Settings.settingChange += SettingChange;
        SettingChange(Settings.values);
    }

    private void OnDestroy()
    {
        Settings.settingChange -= SettingChange;
    }

    private void Update()
    {
        timmer -= Time.deltaTime;
        if (timmer < 0)
        {
            timmer = fallingTime;
            Move(Vector2Int.down, true);
        }

        if (moveCold > 0)//left and right handel
        {
            if (GetKey(Key.left)||GetKey(Key.right))
            {
                moveCold -= Time.deltaTime;
            }
            else
            {
                moveCold = 0;
            }
        }
        if (GetKeyDown(Key.left) && !GetKeyDown(Key.right))
        {
            Move(Vector2Int.left, false);
            moveCold = DAS/1000f;
        }
        if (!GetKeyDown(Key.left)&& GetKeyDown(Key.right))
        {
            Move(Vector2Int.right, false);
            moveCold = DAS / 1000f;
        }
        if (GetKey(Key.left) && !GetKey(Key.right)&& moveCold <= 0)
        {
            Move(Vector2Int.left, false);
            moveCold = ARR / 1000f;
        }
        if (!GetKey(Key.left) && GetKey(Key.right) && moveCold <= 0)
        {
            Move(Vector2Int.right, false);
            moveCold = ARR / 1000f;
        }

        if (!GetKey(Key.x) && GetKeyDown(Key.b) && !GetKey(Key.y))//rotate handel
        {
            bool couldRotate;
            Vector2Int offset;
            Rotations rotation;
            (couldRotate,offset,rotation)=RotateRight();
            if (couldRotate)
            {
                Pos += offset;
                Rotation = rotation;
            }
        }
        if (GetKeyDown(Key.x) && !GetKey(Key.b) && !GetKey(Key.y))
        {
            bool couldRotate;
            Vector2Int offset;
            Rotations rotation;
            (couldRotate, offset, rotation) = RotateLeft();
            if (couldRotate)
            {
                Pos += offset;
                Rotation = rotation;
            }
        }
        if (!GetKey(Key.x) && !GetKey(Key.b)&& GetKeyDown(Key.y))
        {
            bool couldRotate;
            Vector2Int offset;
            Rotations rotation;
            (couldRotate, offset, rotation) = Rotate180();
            if (couldRotate)
            {
                Pos += offset;
                Rotation = rotation;
            }
        }

        if (downCold > 0)//Dwon Handel
        {
            if (!GetKey(Key.down))
            {
                downCold = 0;
            }
            else
            {
                downCold -= Time.deltaTime;
            }
        }
        if (GetKey(Key.down)&& downCold <= 0)
        {
            downCold = ARR_down / 1000f;
            if (Move(Vector2Int.down, false))
            {
                timmer = fallingTime;
            }
        }

        if (GetKeyDown(Key.a))
        {
            Down();
        }

        if (GetKeyDown(Key.up))
        {
            Hold();
        }

        transform.localPosition = Frame.instance.GetPosV2(Pos);
    }

    private (bool,Vector2Int,Rotations) RotateRight()
    {
        Rotations r = Rotate(Rotation,1);

        bool couldRotate;
        Vector2Int offset;
        (couldRotate, offset) = CheckRotate(Rotation,r,Pos,Type);
        if (couldRotate)
        {
            fallingGroupShow.RotateTo(r);
            return (true, offset, r);
        }

        return (false, new Vector2Int(),r);
    }

    private (bool, Vector2Int, Rotations) RotateLeft()
    {
        Rotations r = Rotate(Rotation, -1);

        bool couldRotate;
        Vector2Int offset;
        (couldRotate, offset) = CheckRotate(Rotation, r, Pos, Type);
        if (couldRotate)
        {
            fallingGroupShow.RotateTo(r);
            return (true, offset, r);
        }

        return (false, new Vector2Int(), r);
    }

    private (bool, Vector2Int, Rotations) Rotate180()
    {
        Rotations r = Rotate(Rotation, 1);

        bool couldRotate;
        Vector2Int offset;
        (couldRotate, offset) = CheckRotate(Rotation, r, Pos, Type);
        if (couldRotate)
        {
            Rotations r2 = Rotate(r, 1);
            bool couldRotate2;
            Vector2Int offset2;
            (couldRotate2, offset2) = CheckRotate(r, r2, Pos+offset, Type);
            if (couldRotate2)
            {
                fallingGroupShow.RotateTo(r2);
                return (true, offset+offset2, r2);
            }
            else
            {
                fallingGroupShow.RotateTo(r);
                return (true, offset, r);
            }
        }
        else
        {
            return (false, new Vector2Int(), r);
        }
    }
    private static (bool, Vector2Int) CheckRotate(Rotations origin, Rotations target,Vector2Int pos,GroupTypes type)
    {
        int[,] checks;
        if (type == GroupTypes.I)
        {
            checks = rotareCheckI[(origin, target)];
        }
        else
        {
            checks = rotareCheck[(origin, target)];
        }
        for (int i = 0; i < checks.GetLength(0); i++)
        {
            Vector2Int offset = new(checks[i, 0], checks[i, 1]);
            if (CheckPosFit(pos + offset, type, target))
            {
                return (true, offset);
            }
        }
        return (false, new Vector2Int());
    }

    private Rotations Rotate(Rotations rotation,int direction)
    {
        direction = (direction % 4 + 4) % 4;
        return (Rotations)(((int)rotation + direction) % 4);
    }

    private bool Move(Vector2Int move,bool set)
    {
        bool fit;

        fit = CheckPosFit(Pos + move, Type, Rotation);

        if (fit)
        {
            Pos += move;
            return true;
        }
        else if(set)
        {
            SetBlock(Pos, Type, Rotation, BlockID);
            Reload();
            return false;
        }
        else
        {
            return false;
        }
    }
    private void Down()
    {
        while (Move(Vector2Int.down, true)) ;
    }
    private void Hold()
    {
        if (!couldHold)
        {
            return;
        }
        couldHold = false;
        if (isHolding)
        {
            GroupTypes tmp = Holding;
            Holding = Type;
            Type = tmp;
        }
        else
        {
            Holding = Type;
            Type = GetOneFromWaiting();
            OnReload?.Invoke();
            isHolding = true;
        }
        fallingGroupShow.SetType(Type);
        OnHold?.Invoke();
    }

    private static void SetBlock(Vector2Int pos, GroupTypes type, Rotations rotation, int[] blockID)
    {
        int[,,] dat = groupDic[type];
        if (blockID.Length != 4)
        {
            Debug.LogError("Wrong blockID length!");
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            Vector2Int blockPos = new(dat[(int)rotation, i, 0] + pos.x, dat[(int)rotation, i, 1] + pos.y);
            Ground.Instance.SetBlock(blockPos, blockID[i]);
        }
    }

    private static bool CheckPosFit(Vector2Int pos,GroupTypes type,Rotations rotation)
    {
        int[,,] dat = groupDic[type];
        for (int i = 0; i < 4; i++)
        {
            Vector2Int blockPos = new(dat[(int)rotation, i, 0]+pos.x, dat[(int)rotation, i, 1]+pos.y);
            if (Ground.Instance.GetBlockID(blockPos) > 0)
            {
                return false;
            }
        }
        return true;
    }
}