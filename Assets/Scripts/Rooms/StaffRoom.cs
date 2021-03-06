using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRoom : BaseWorkerRoom
{
    private Housemaid _Housemaid = null;

    public StaffRoomInfoPanel InfoPanel;

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
        if (NewWorker == null)
        {
            return;
        }

        if (NewWorker.GetComponent<Housemaid>() != null
           && _Housemaid == null)
        {
            InfoPanel.activate_HousemaidImage();
            _Housemaid = (NewWorker as Housemaid);
        }
    }

}
