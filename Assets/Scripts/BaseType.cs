using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseType : MonoBehaviour
{
    public int CurFloor;
    protected LevelManager _LM;
    protected Scheduler _Scheduler;
    protected TimeCounter _TimeCounter;
    protected MoneyManager _MoneyManager;
    protected ReputationManager _RepManager;

    protected virtual void Awake()
    {
        CurFloor = 0;
        init_LM();
        init_Scheduler();
        init_TimeCounter();
        init_MoneyManager();
        init_RepManager();
    }

    private void init_LM()
    {
        _LM = Object.FindObjectOfType<LevelManager>();
    }

    private void init_Scheduler()
    {
        _Scheduler = Object.FindObjectOfType<Scheduler>();
    }

    private void init_TimeCounter()
    {
        _TimeCounter = Object.FindObjectOfType<TimeCounter>();
    }

    private void init_MoneyManager()
    {
        _MoneyManager = FindObjectOfType<MoneyManager>();
    }

    private void init_RepManager()
    {
        _RepManager = FindObjectOfType<ReputationManager>();
    }

}
