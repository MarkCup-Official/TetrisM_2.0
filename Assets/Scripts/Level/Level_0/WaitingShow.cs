using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingShow : MonoBehaviour
{
    public int id;

    public FallingBlock fb;

    private FallingGroupShow show;

    private void Awake()
    {
        show = GetComponent<FallingGroupShow>();
        fb.OnReload += Reload;
    }

    private void Reload()
    {
        show.Show(fb.waiting[id], FallingBlock.Rotations.Zero);
    }
}
