using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRoom : BaseWorkerRoom
{
    private Housemaid _Housemaid = null;

    public override bool is_worker_on_this_pos_exist(LevelManager.TypeOfWorker WorkerType)
    {
        if (_Housemaid != null)
        {
            return true;
        }
        return false;
    }

    public override void add_worker(BaseWorker NewWorker)
    {
        if(NewWorker.GetComponent<Housemaid>() != null
           && _Housemaid == null)
        {
            _Housemaid = (NewWorker as Housemaid);
        }
    }

}
