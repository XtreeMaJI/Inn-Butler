using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : BaseWorker
{
    public enum TypeOfServant: int
    {
        Kitchen = 0,
        Bar = 1,
        Reception = 2,
        None = 3
    }

    private TypeOfServant ServantType = TypeOfServant.None;

    private Transform _PosForCarry;

    private Carryable _Carryable = null;

    private new void Start()
    {
        _PosForCarry = transform.Find("PosForCarry");
        _MoneyManager.increase_TotalSalary(Salary);
    }


    protected override void do_work()
    {
        switch(ServantType)
        {
            case TypeOfServant.Kitchen:
                Destroy(_Carryable.gameObject);
                _Carryable = null;
                _RoomForWork.refill_Food();
                _RoomForWork = null;
                change_state(StateOfCharacter.MoveToWorkerRoom, CurRoom);
                break;
            case TypeOfServant.Bar:
                Destroy(_Carryable.gameObject);
                _Carryable = null;
                _RoomForWork.refill_Fun();
                _RoomForWork = null;
                change_state(StateOfCharacter.MoveToWorkerRoom, CurRoom);
                break;
            case TypeOfServant.Reception:
                if((CurRoom as Reception).is_VisitorBuf_empty() == false)
                {
                    (CurRoom as Reception).check_visitor_in_suitable_room();
                }
                break;
        }
        
    }

    protected override void find_work()
    {
        switch(ServantType)
        {
            case TypeOfServant.Kitchen:
                if(_RoomForWork != null)
                {
                    if ((CurRoom as Kitchen).FinishedDish != null)
                    {
                        _Carryable = (CurRoom as Kitchen).FinishedDish.grab(_PosForCarry);
                        change_state(StateOfCharacter.MoveToPlaceForWork, _RoomForWork);
                    }
                    break;
                }
                _RoomForWork = _Scheduler.get_room_from_queue(CurRoom);
                _Scheduler.delete_room_from_FoodQueue(_RoomForWork);
                break;
            case TypeOfServant.Bar:
                if(_RoomForWork != null)
                {
                    _Carryable = (CurRoom as Bar).try_take_wine();
                    if(_Carryable != null)
                    {
                        _Carryable.grab(_PosForCarry);
                        change_state(StateOfCharacter.MoveToPlaceForWork, _RoomForWork);
                    }
                    break;
                }
                _RoomForWork = _Scheduler.get_room_from_queue(CurRoom);
                _Scheduler.delete_room_from_WineQueue(_RoomForWork);
                break;
            case TypeOfServant.Reception:
                transform.SetPositionAndRotation((CurRoom as Reception).PosForWork, new Quaternion());
                change_state(StateOfCharacter.Working);
                break;
        }
    }

    public override void set_WorkerRoom(BaseWorkerRoom RoomForWorker)
    {
        if(RoomForWorker == null)
        {
            return;
        }

        if(RoomForWorker.GetComponent<Kitchen>() != null)
        {
            ServantType = TypeOfServant.Kitchen;
            CurRoom = RoomForWorker;
            RoomForWorker.add_worker(this);
            change_state(StateOfCharacter.MoveToWorkerRoom, RoomForWorker);
        }
        if (RoomForWorker.GetComponent<Reception>() != null)
        {
            ServantType = TypeOfServant.Reception;
            CurRoom = RoomForWorker;
            RoomForWorker.add_worker(this);
            change_state(StateOfCharacter.MoveToWorkerRoom, RoomForWorker);
        }
        if (RoomForWorker.GetComponent<Bar>() != null)
        {
            ServantType = TypeOfServant.Bar;
            CurRoom = RoomForWorker;
            RoomForWorker.add_worker(this);
            change_state(StateOfCharacter.MoveToWorkerRoom, RoomForWorker);
        }
    }

}
