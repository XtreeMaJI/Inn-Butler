using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseType : MonoBehaviour
{
    public int CurFloor;
    protected LevelManager _LM;
    protected Scheduler _Scheduler;

    protected virtual void Awake()
    {
        CurFloor = 0;
        init_LM();
        init_Scheduler();
    }

    private void init_LM()
    {
        _LM = Object.FindObjectOfType<LevelManager>();
    }

    private void init_Scheduler()
    {
        _Scheduler = Object.FindObjectOfType<Scheduler>();
    }

}
