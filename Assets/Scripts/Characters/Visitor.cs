using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : BaseCharacter
{
    public enum StateOfVisitor: int
    {
        Idle = 0,
        MoveToReception = 1,
        MoveToRoom = 2,
        FollowPerson = 3
    }

    public LevelManager.TypeOfVisitor VisitorType { get; private set; }

    public int LeaseTerm; //Время, на которое посетитель займёт комнату

    protected void Start()
    {
        Reception Rec = Object.FindObjectOfType<Reception>();
        change_state(StateOfCharacter.MoveToReception, Rec);
        init_Visitor();
    }

    private void init_Visitor()
    {
        if(GetComponent<Traveller>() != null)
        {
            VisitorType = LevelManager.TypeOfVisitor.Traveller;
        }
        if (GetComponent<Citizen>() != null)
        {
            VisitorType = LevelManager.TypeOfVisitor.Citizen;
        }
        if(GetComponent<Merchant>() != null)
        {
            VisitorType = LevelManager.TypeOfVisitor.Merchant;
        }
    }

    protected void Update()
    {
        switch (CharacterState)
        {
            case StateOfCharacter.Idle:
                break;
            case StateOfCharacter.MoveToRoom:
                move_to_GlobalTarget();
                if (isGlobalTargetReached == true)
                {
                    reset_state();
                }
                break;
            case StateOfCharacter.MoveToReception:
                move_to_GlobalTarget();
                //Если добрались до ресепшена
                if (isGlobalTargetReached == true)
                {
                    reset_state();
                    RoomBuf.GetComponent<Reception>().set_Visitor(this.GetComponent<Visitor>());
                }
                break;
            case StateOfCharacter.FollowPerson:
                follow_GlobalTarget();
                break;
        }
    }

}
