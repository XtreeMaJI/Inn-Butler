using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : BaseCharacter
{
    public LevelManager.TypeOfVisitor VisitorType { get; private set; }

    private float TimeForWalk; //Время, затрачиваемое на прогулку

    protected void Start()
    {
        Reception Rec = Object.FindObjectOfType<Reception>();
        change_state(StateOfCharacter.MoveToReception, Rec);
        init_Visitor();

        TimeForWalk = 2 * LevelManager.DayLength / 24;
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
            case StateOfCharacter.MoveToOwnRoom:
                if (isGlobalTargetReached == true && CurRoom != null &&
                   (CurRoom as LivingRoom).RoomState == LivingRoom.StateOfLivingRoom.Empty)
                {
                    (CurRoom as LivingRoom).RoomState = LivingRoom.StateOfLivingRoom.VisitorInside;
                    reset_state();
                    break;
                }
                move_to_GlobalTarget();
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
            case StateOfCharacter.MoveToExit:
                move_to_GlobalTarget();
                if (isGlobalTargetReached == true)
                {
                    StartCoroutine("wait_outside");
                    reset_state();
                }
                break;
        }
    }

    public void go_for_walk()
    {
        change_state(StateOfCharacter.MoveToExit, _ExitPos);
        (CurRoom as LivingRoom).RoomState = LivingRoom.StateOfLivingRoom.Empty;
    }

    private IEnumerator wait_outside()
    {
        yield return new WaitForSeconds(TimeForWalk);
        change_state(StateOfCharacter.MoveToOwnRoom, CurRoom);
    }

}
