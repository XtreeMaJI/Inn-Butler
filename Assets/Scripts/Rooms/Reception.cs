using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : BaseWorkerRoom
{
    private Servant _Servant = null;

    public Vector3 PosForWork;

    private void Start()
    {
        _TakeKeysB = _Can.transform.Find("TakeKeysB").gameObject;
        _RejectB = _Can.transform.Find("RejectB").gameObject;
        PosForWork = transform.Find("PosForWork").position;
        _VisitorBuf = null;
        _PlayerBuf = null;
    }

    //Проводить посетителя к комнате
    public void take_visitor_to_room()
    {
        if (_VisitorBuf != null && 
            (_PlayerBuf != null && _PlayerBuf.GetComponent<PlayerController>().FollowingVisitor == null))
        {
            _VisitorBuf.change_state(BaseCharacter.StateOfCharacter.FollowPerson, _PlayerBuf.GetComponent<BaseType>());
            _LM.VisInQueue = null;
            _LM.create_visitor_if_possible();
            _PlayerBuf.GetComponent<PlayerController>().FollowingVisitor = _VisitorBuf;
            disable_buttons();
        }
    }

    public void reject_to_visitor()
    {
        Destroy(_VisitorBuf.gameObject);
        disable_buttons();
    }

    public void set_Visitor(Visitor NewVisitor)
    {
        _VisitorBuf = NewVisitor;
        enable_buttons();
    }

    public override bool is_worker_on_this_pos_exist(LevelManager.TypeOfWorker WorkerType)
    {
        if(_Servant != null)
        {
            return true;
        }
        return false;
    }

    public override void add_worker(BaseWorker NewWorker)
    {
        if(NewWorker != null && _Servant == null)
        {
            _Servant = (NewWorker as Servant);
        }
    }

    public bool is_VisitorBuf_empty()
    {
        if(_VisitorBuf == null)
        {
            return true;
        }
        return false;
    }

    //Работает для посетителя в VisitorBuf
    public void check_visitor_in_suitable_room()
    {
        if(_VisitorBuf == null)
        {
            return;
        }

        List<Room> RoomList = _LM.get_RoomList_copy();

        foreach(Room iRoom in RoomList)
        {
            if(iRoom.is_Suitable_for_TypeOfVisitor(_VisitorBuf.VisitorType) &&
               iRoom.Vis == null)
            {
                (iRoom as LivingRoom).check_visitor_in(_VisitorBuf);
                _VisitorBuf = null;
                _LM.VisInQueue = null;
                _LM.create_visitor_if_possible();
                return;
            }
        }
    }

}
