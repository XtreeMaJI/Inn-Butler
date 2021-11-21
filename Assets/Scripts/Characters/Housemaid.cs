using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housemaid : BaseWorker
{
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
            _RoomForWork.change_state(LivingRoom.StateOfLivingRoom.AwaitForCleaning);
            return;
        }
    }

    protected override void do_work()
    {
        if(_RoomForWork.Clean >= _RoomForWork.MaxClean)
        {
            _Animator.SetBool("IsCleaning", false);
            change_state(StateOfCharacter.MoveToWorkerRoom, CurRoom);
            _RoomForWork = null;
            increase_XP();
            return;
        }
        if (_RoomForWork.RoomState != LivingRoom.StateOfLivingRoom.Cleaning)
        {
            _RoomForWork.StartCleaning(WorkSpeedMod);
            transform.SetPositionAndRotation(_RoomForWork.transform.position, new Quaternion());
            _Animator.SetBool("IsCleaning", true);
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
        PlaceInWorkerRoom = RoomForWorker.transform.position;
        DirInWorkerRoom = new Quaternion(0, 0, 0, 1);
    }

    protected override void change_XP_on_UI()
    {
        if (Level + 1 >= XPForLevel.Length)
        {
            (CurRoom as StaffRoom).InfoPanel.set_HousemaidXP(Level, CurXP, 0);
            return;
        }
        (CurRoom as StaffRoom).InfoPanel.set_HousemaidXP(Level, CurXP, XPForLevel[Level + 1]);
    }

}
