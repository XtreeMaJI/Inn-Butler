using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : BaseType
{
    public enum StateOfCharacter : int
    {
        Idle = 0,
        MoveToReception = 1,
        MoveToRoom = 2,
        FollowPerson = 3,
        MoveToHall = 4,
        MoveToExit = 5
    }

    public Room CurRoom;

    public BaseType GlobalTarget; //Пункт назначения(персонаж или комната)
    public BaseType LocalTarget; //Промежуточный пункт на пути к GlobalTarget(лестница или комната)

    public BaseType RoomBuf;

    public StateOfCharacter CharacterState;

    public float Speed = 1f;

    private Rigidbody2D _rb;

    protected bool isGlobalTargetReached;

    protected override void Awake()
    {
        base.Awake();

        GlobalTarget = null;
        LocalTarget = null;
        CurRoom = null;

        _rb = GetComponent<Rigidbody2D>();

        RoomBuf = null;
        isGlobalTargetReached = false;
    }

    protected void Update()
    {
        switch (CharacterState)
        {
            case StateOfCharacter.Idle:
                break;
            case StateOfCharacter.MoveToRoom:
                move_to_GlobalTarget();
                if(isGlobalTargetReached == true)
                {
                    reset_state();
                }
                break;
            case StateOfCharacter.MoveToReception:
                move_to_GlobalTarget();
                //Если добрались до ресепшена
                if(isGlobalTargetReached == true)
                {
                    reset_state();
                    RoomBuf.GetComponent<Reception>().VisitorBuf = this.GetComponent<Visitor>();
                }
                break;
            case StateOfCharacter.FollowPerson:
                follow_GlobalTarget();
                break;
        }
    }

    private void move_to_GlobalTarget()
    {
        if(LocalTarget == null)
        {
            find_LocalTarget();
        }
        else if(is_local_target_reached())
        {
            if(LocalTarget == GlobalTarget)
            {
                isGlobalTargetReached = true;
            } else if(RoomBuf.GetComponent<Stairs>() != null)
            {
                if (GlobalTarget.CurFloor < CurFloor)
                {
                    RoomBuf.GetComponent<Stairs>().climb_down(this.gameObject);
                    LocalTarget = null;
                }
                else if (GlobalTarget.CurFloor > CurFloor)
                {
                    RoomBuf.GetComponent<Stairs>().climb_up(this.gameObject);
                    LocalTarget = null;
                }
            }
        }
        else
        {
            float dir = (LocalTarget.transform.position.x - transform.position.x) / Mathf.Abs(LocalTarget.transform.position.x - transform.position.x);
            _rb.transform.Translate(new Vector3(dir * Speed * Time.deltaTime, 0f, 0f));
        }
    }

    private void follow_GlobalTarget()
    {
        find_LocalTarget();
        if (is_local_target_reached())
        {
            if (LocalTarget == GlobalTarget)
            {
                isGlobalTargetReached = true;
            }
            else if (RoomBuf.GetComponent<Stairs>() != null)
            {
                if (GlobalTarget.CurFloor < CurFloor)
                {
                    RoomBuf.GetComponent<Stairs>().climb_down(this.gameObject);
                    LocalTarget = null;
                }
                else if (GlobalTarget.CurFloor > CurFloor)
                {
                    RoomBuf.GetComponent<Stairs>().climb_up(this.gameObject);
                    LocalTarget = null;
                }
            }
        }
        else
        {
            float dir = (LocalTarget.transform.position.x - transform.position.x) / Mathf.Abs(LocalTarget.transform.position.x - transform.position.x);
            _rb.transform.Translate(new Vector3(dir * Speed * Time.deltaTime, 0f, 0f));
        }
    }

    private void find_LocalTarget()
    {
        if (GlobalTarget.CurFloor < CurFloor)
        {
            LocalTarget = find_closest_stair("down");
        }
        else if (GlobalTarget.CurFloor > CurFloor)
        {
            LocalTarget = find_closest_stair("up");
        }
        else
        {
            LocalTarget = GlobalTarget;
        }
    }

    //dir - "up" or "down"
    Stairs find_closest_stair(string dir)
    {
        List<Stairs> StairList = _LM.get_all_stairs_on_floor(CurFloor);
        Stairs ClosestStair = null;
        if (dir == "up")
        {
            foreach (Stairs stairs in StairList)
            {
                if (stairs.UpperStair == null)
                {
                    StairList.Remove(stairs);
                }
            }
        }
        else if (dir == "down")
        {
            foreach (Stairs stairs in StairList)
            {
                if (stairs.LowerStair == null)
                {
                    StairList.Remove(stairs);
                }
            }
        }

        float MinDistance = Mathf.Abs(StairList[0].transform.position.x - transform.position.x);
        foreach(Stairs stairs in StairList)
        {
            if(Mathf.Abs(stairs.transform.position.x - transform.position.x) <= MinDistance)
            {
                MinDistance = Mathf.Abs(stairs.transform.position.x - transform.position.x);
                ClosestStair = stairs;
            }
        }
        return ClosestStair;
    }

    private bool is_local_target_reached()
    { 
        if(LocalTarget != null && RoomBuf == LocalTarget)
        {
            return true;
        }
        if (CharacterState == StateOfCharacter.FollowPerson && LocalTarget != null && 
            LocalTarget == GlobalTarget && is_GlobalTarget_in_range(_LM.RoomWidth/4))
        {
            return true;
        }
        return false;
    }

    public void change_state(StateOfCharacter NewState, BaseType NewGlobalTarget)
    {
        CharacterState = NewState;
        GlobalTarget = NewGlobalTarget;
        LocalTarget = null;
        isGlobalTargetReached = false;
    }

    private void reset_state()
    {
        GlobalTarget = null;
        LocalTarget = null;
        CharacterState = StateOfCharacter.Idle;
    }

    private bool is_GlobalTarget_in_range(float range)
    {
        if((transform.position.x + range > GlobalTarget.transform.position.x && transform.position.x < GlobalTarget.transform.position.x) ||
           (transform.position.x - range < GlobalTarget.transform.position.x && transform.position.x > GlobalTarget.transform.position.x))
        {
            return true;
        }
        return false;
    }

    

}
