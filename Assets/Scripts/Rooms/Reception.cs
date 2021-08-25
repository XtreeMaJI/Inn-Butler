using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : BaseWorkerRoom
{

    private void Start()
    {
        _TakeKeysB = _Can.transform.Find("TakeKeysB").gameObject;
        _RejectB = _Can.transform.Find("RejectB").gameObject;
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

}
