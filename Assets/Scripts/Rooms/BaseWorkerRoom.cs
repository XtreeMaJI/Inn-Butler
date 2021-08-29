using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWorkerRoom : Room
{

    protected new void Awake()
    {
        base.Awake();
        _AddStaffB = _Can.transform.Find("AddStaffB").gameObject;
    }

    public void press_AddStaffB()
    {
        _LM.ui.open_AddStaffPanel(this);
    }

    public abstract bool is_worker_on_this_pos_exist(LevelManager.TypeOfWorker WorkerType);

    public abstract void add_worker(BaseWorker NewWorker);

}
