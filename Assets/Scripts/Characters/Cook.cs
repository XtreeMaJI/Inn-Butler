using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : BaseWorker
{
    private Vector2 _PosForWork = new Vector2(0f, 0f);

    protected override void do_work()
    {
        (CurRoom as Kitchen).start_cooking(WorkSpeedMod); 
    }

    protected override void find_work()
    {
        //Переместить к котлу, заставить работать   
        transform.SetPositionAndRotation(_PosForWork, new Quaternion());
        change_state(StateOfCharacter.Working);
    }
    public override void set_WorkerRoom(BaseWorkerRoom RoomForWorker)
    {
        if (RoomForWorker.GetComponent<Kitchen>() == null)
        {
            return;
        }
        this.CurRoom = RoomForWorker;
        RoomForWorker.add_worker(this);
        change_state(StateOfCharacter.MoveToWorkerRoom, RoomForWorker);
        _PosForWork = RoomForWorker.transform.Find("PosForWork").position;
    }
}
