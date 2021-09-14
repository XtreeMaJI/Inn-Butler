using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : BaseWorker
{
    protected override void do_work()
    {
        if((CurRoom as Kitchen).FinishedDish != null)
        {
            change_state(StateOfCharacter.Idle);
            _Animator.SetBool("IsCooking", false);
            return;
        }
        (CurRoom as Kitchen).start_cooking(WorkSpeedMod); 
    }

    protected override void find_work()
    {
        if((CurRoom as Kitchen).FinishedDish == null)
        {
            change_state(StateOfCharacter.Working);
            _Animator.SetBool("IsCooking", true);
        }
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
        PlaceInWorkerRoom = RoomForWorker.transform.Find("PosForWork").position;
        DirInWorkerRoom = new Quaternion(0, 0, 0, 1);
    }
}
