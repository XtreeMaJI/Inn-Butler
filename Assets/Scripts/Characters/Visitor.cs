using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Visitor : BaseCharacter
{
    public LevelManager.TypeOfVisitor VisitorType { get; private set; }

    private float TimeForWalk; //Время, затрачиваемое на прогулку

    private bool IsWaitForWalk = false; //Запланировал ли посетитель прогулку

    public int NumOfDaysBeforeLeave { private set; get; }
    private int TotalNumOfDays; //Сколько дней посетитель провёл в таверне

    private int GivingReputation = 0; //На сколько увеличится репутация после отъезда посетителя

    //Объекты корутин во время ожидания прогулки и самой прогулки
    Coroutine WaitingOutside = null;
    Coroutine WaitingForWalk = null;

    private Hall _FreeHall = null;

    protected void Start()
    {
        Reception Rec = Object.FindObjectOfType<Reception>();
        change_state(StateOfCharacter.MoveToReception, Rec);
        init_Visitor();

        TimeForWalk = 2 * _TimeCounter.SecondsInGameHour;

        NumOfDaysBeforeLeave = Random.Range(1, 4);
        TotalNumOfDays = NumOfDaysBeforeLeave;

        _TimeCounter.DayStarter += handle_day_start;
    }

    private void init_Visitor()
    {
        if(GetComponent<Traveller>() != null)
        {
            VisitorType = LevelManager.TypeOfVisitor.Traveller;
            GivingReputation = 5;
        }
        if (GetComponent<Citizen>() != null)
        {
            VisitorType = LevelManager.TypeOfVisitor.Citizen;
            GivingReputation = 10;
        }
        if(GetComponent<Merchant>() != null)
        {
            VisitorType = LevelManager.TypeOfVisitor.Merchant;
            GivingReputation = 20;
        }
    }

    protected void Update()
    {
        switch (CharacterState)
        {
            case StateOfCharacter.Idle:
                if (RoomBuf == CurRoom)
                {
                    set_new_activity();
                }
                break;
            case StateOfCharacter.MoveToOwnRoom:
                if (isGlobalTargetReached == true && CurRoom != null &&
                   (CurRoom as LivingRoom).RoomState == LivingRoom.StateOfLivingRoom.Empty)
                {
                    transform.SetPositionAndRotation(CurRoom.transform.position, new Quaternion());
                    (CurRoom as LivingRoom).change_state(LivingRoom.StateOfLivingRoom.VisitorInside);
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
                    WaitingOutside = StartCoroutine("wait_outside");
                    reset_state();
                }
                break;
            case StateOfCharacter.ReturnToPay:
                if (isGlobalTargetReached == true && CurRoom != null)
                {
                    pay_for_room();
                    (CurRoom as LivingRoom).free_the_room();
                    change_state(StateOfCharacter.LeaveTavern, _ExitPos);
                    break;
                }
                move_to_GlobalTarget();
                break;
            case StateOfCharacter.LeaveTavern:
                move_to_GlobalTarget();
                if (isGlobalTargetReached == true)
                {
                    _TimeCounter.DayStarter -= handle_day_start;
                    Destroy(this.gameObject);
                }
                break;
            case StateOfCharacter.MoveToHall:
                if (isGlobalTargetReached == true)
                {
                    transform.SetPositionAndRotation(RoomBuf.transform.position, new Quaternion());
                    change_state(StateOfCharacter.StayInHall);
                    _FreeHall.enter_the_room(this);
                    break;
                }
                move_to_GlobalTarget();
                break;
            case StateOfCharacter.StayInHall:
                if (_TimeCounter.CurrentHour > 18)
                {
                    change_state(StateOfCharacter.MoveToOwnRoom, CurRoom);
                    _FreeHall.free_space_in_room(this);
                    _FreeHall = null;
                }
                break;
        }
    }

    public void go_for_walk()
    {
        change_state(StateOfCharacter.MoveToExit, _ExitPos);
        (CurRoom as LivingRoom).change_state(LivingRoom.StateOfLivingRoom.Empty);
        (CurRoom as LivingRoom).handle_visitor_leave_room();
    }

    private IEnumerator wait_outside()
    {
        yield return new WaitForSeconds(TimeForWalk);
        change_state(StateOfCharacter.MoveToOwnRoom, CurRoom);
        IsWaitForWalk = false;
    }

    //Если не ночь, то посетитель ищет свободный Hall или планирует прогулку
    private void set_new_activity()
    {
        if(CurRoom == null || IsWaitForWalk == true)
        {
            return;
        }

        if (_TimeCounter.CurrentHour > 18)
        {
            return;
        }

        _FreeHall = get_free_Hall();

        if (_FreeHall != null)
        {
            if(WaitingForWalk != null)
            {
                StopCoroutine(WaitingForWalk);
            }
            _FreeHall.reserve_place_in_room(this);
            change_state(StateOfCharacter.MoveToHall, _FreeHall);
            (CurRoom as LivingRoom).change_state(LivingRoom.StateOfLivingRoom.Empty);
            (CurRoom as LivingRoom).handle_visitor_leave_room();
        }

        if (_TimeCounter.CurrentHour > 3)
        {
            return;
        }

        WaitingForWalk = StartCoroutine("wait_for_walk");
    }

    private Hall get_free_Hall()
    {
        List<Hall> HallList = _LM.get_HallList();
        HallList = HallList.FindAll(LHall => LHall.HasFreeSpace == true);
        Hall NewFreeHall = null;

        if(HallList.Count == 0)
        {
            return NewFreeHall;
        }
        float x = transform.position.x;
        float MinDistance = Mathf.Abs(x - HallList[0].transform.position.x);
        NewFreeHall = HallList[0];

        foreach (Hall LHall in HallList)
        {
            float Distance = Mathf.Abs(x - LHall.transform.position.x);
            if (Distance < MinDistance)
            {
                MinDistance = Distance;
                NewFreeHall = LHall;
            }
        }
        return NewFreeHall;
    }

    private IEnumerator wait_for_walk()
    {
        int RandomNum = Random.Range(1, 6);
        IsWaitForWalk = true;
        yield return new WaitForSeconds(RandomNum * _TimeCounter.SecondsInGameHour);
        go_for_walk();
        WaitingForWalk = null;
    }

    private void handle_day_start()
    {
        if(CurRoom == null)
        {
            leave_tavern();
            return;
        }

        NumOfDaysBeforeLeave--;
        (CurRoom as LivingRoom).InfoPanel.set_DaysBeforeLeave(NumOfDaysBeforeLeave);

        if(NumOfDaysBeforeLeave == 0)
        {
            leave_tavern();
        }
    }

    public void leave_tavern()
    {
        if(CurRoom == null)
        {
            change_state(StateOfCharacter.LeaveTavern, _ExitPos);
            if(RoomBuf != null)
            {
                Reception ReceptionBuf = RoomBuf.GetComponent<Reception>();
                if (ReceptionBuf != null)
                {
                    ReceptionBuf.clear_VisitorBuf();
                }
            }
            return;
        }
        change_state(StateOfCharacter.ReturnToPay, CurRoom);
        if(WaitingForWalk != null)
        {
            StopCoroutine(WaitingForWalk);
        }
        if (WaitingOutside != null)
        {
            StopCoroutine(WaitingOutside);
        }
    }

    private void pay_for_room()
    {
        _MoneyManager.increase_money(TotalNumOfDays*CurRoom.RoomPayment);
        _RepManager.increase_reputation(GivingReputation);
    }

}
