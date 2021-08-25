using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkerRoom : Room
{
    protected new void Awake()
    {
        base.Awake();
        _AddStaffB = _Can.transform.Find("AddStaffB").gameObject;
    }

    public void press_AddStaffB()
    {
        _LM.ui.open_AddStaffPanel(RoomType);
    }
}
