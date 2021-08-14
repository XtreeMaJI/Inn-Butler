using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : Room
{
    private GameObject _TakeKeysB;
    public Visitor VisitorBuf;

    private BaseType _PlayerBuf;

    private void Start()
    {
        _TakeKeysB = _Can.transform.Find("TakeKeysB").gameObject;
        VisitorBuf = null;
        _PlayerBuf = null;
    }

    //Проводить посетителя к комнате
    public void take_visitor_to_room()
    {
        if (VisitorBuf != null && 
            (_PlayerBuf != null && _PlayerBuf.GetComponent<PlayerController>().FollowingVisitor == null))
        {
            VisitorBuf.change_state(BaseCharacter.StateOfCharacter.FollowPerson, _PlayerBuf);
            _LM.VisInQueue = null;
            _LM.create_visitor_if_possible();
            _PlayerBuf.GetComponent<PlayerController>().FollowingVisitor = VisitorBuf;
        }
    }

    private new void toggle_buttons()
    {
        if (_TakeKeysB.gameObject.activeSelf)
        {
            _TakeKeysB.gameObject.SetActive(false);
        }
        else
        {
            _TakeKeysB.gameObject.SetActive(true);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {      
            toggle_buttons();
            _PlayerBuf = collision.GetComponent<BaseType>();
        }
        else if (collision.tag == "Character")
        {
            collision.GetComponent<BaseCharacter>().RoomBuf = this;
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            toggle_buttons();
            _PlayerBuf = null;
        }
        else if (collision.tag == "Character")
        {
            collision.GetComponent<BaseCharacter>().RoomBuf = null;
        }
    }

}
