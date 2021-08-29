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
        _RoomForWork = _Scheduler.get_closest_room_from_queue(CurRoom);
        if(_RoomForWork != null && _RoomForWork.RoomState == LivingRoom.StateOfLivingRoom.Empty)
        {
            change_state(StateOfCharacter.MoveToPlaceForWork, _RoomForWork);
            _RoomForWork.RoomState = LivingRoom.StateOfLivingRoom.AwaitForCleaning;
        }
    }

    protected override void do_work()
    {
        if(_RoomForWork.Clean >= _RoomForWork.MaxClean)
        {
            change_state(StateOfCharacter.MoveToWorkerRoom, CurRoom);
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
