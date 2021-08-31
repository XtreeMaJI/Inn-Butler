using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housemaid : BaseWorker
{

    private void Start()
    {
        change_state(StateOfCharacter.MoveToWorkerRoom, CurRoom);
    }

    protected override void find_work()
    {
        if(_RoomForWork == null)
        {
            _RoomForWork = _Scheduler.get_room_from_queue(CurRoom);
            _Scheduler.delete_room_from_CleanQueue(_RoomForWork);
        }

        if(_RoomForWork != null && _RoomForWork.RoomState == LivingRoom.StateOfLivingRoom.Empty)
        {
            change_state(StateOfCharacter.MoveToPlaceForWork, _RoomForWork);
            _RoomForWork.RoomState = LivingRoom.StateOfLivingRoom.AwaitForCleaning;
            return;
        }
    }

    protected override void do_work()
    {
        if(_RoomForWork.Clean >= _RoomForWork.MaxClean)
        {
            change_state(StateOfCharacter.MoveToWorkerRoom, CurRoom);
            _RoomForWork = null;
            return;
        }
        if (_RoomForWork.RoomState != LivingRoom.StateOfLivingRoom.Cleaning)
        {
            _RoomForWork.StartCleaning(WorkSpeedMod);
        } 
    }

    public override void set_WorkerRoom(BaseWorkerRoom RoomForWorker)
    {
        if(RoomForWorker.GetComponent<StaffRoom>() == null)
        {
            return;
        }
        this.CurRoom = RoomForWorker;
        RoomForWorker.add_worker(this);
        change_state(StateOfCharacter.MoveToWorkerRoom, RoomForWorker);
    }

}
