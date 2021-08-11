using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseType : MonoBehaviour
{
    public int CurFloor;
    protected LevelManager _LM;

    protected virtual void Awake()
    {
        CurFloor = 0;
        init_LM();
    }

    private void init_LM()
    {
        _LM = Object.FindObjectOfType<LevelManager>();
    }

}
