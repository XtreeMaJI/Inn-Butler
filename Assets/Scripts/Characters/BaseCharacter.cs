using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : BaseType
{
    public enum StateOfCharacter : int
    {
        Idle = 0,
        MoveToReception = 1,
        MoveToOwnRoom = 2,
        FollowPerson = 3,
        MoveToHall = 4,
        MoveToExit = 5,
        MoveToWorkerRoom = 6,
        MoveToPlaceForWork = 7,
        Working = 8,
        ReturnToPay = 9, //Возвращение в комнату, чтобы расплатиться и уйти
        LeaveTavern = 10,
        StayInHall = 11
    }

    public Room CurRoom { get; set; } //Комната, к которой привязан персонаж(рабочая или жилая)

    private BaseType GlobalTarget; //Пункт назначения(персонаж или комната)
    private BaseType LocalTarget; //Промежуточный пункт на пути к GlobalTarget(лестница или комната)

    public BaseType RoomBuf;

    [SerializeField]protected StateOfCharacter CharacterState;

    private float _Speed = 1f;

    private Rigidbody2D _rb;

    protected bool isGlobalTargetReached;

    protected BaseType _ExitPos;

    protected Animator _Animator;

    private string _BaseSortingLayer;

    private SpriteRenderer _SpriteRender;

    protected override void Awake()
    {
        base.Awake();

        GlobalTarget = null;
        LocalTarget = null;
        CurRoom = null;

        _rb = GetComponent<Rigidbody2D>();

        RoomBuf = null;
        isGlobalTargetReached = false;

        _ExitPos = Object.FindObjectOfType<TavernExit>();

        _Animator = GetComponent<Animator>();

        _SpriteRender = GetComponent<SpriteRenderer>();
        _BaseSortingLayer = _SpriteRender.sortingLayerName;
    }

    protected void move_to_GlobalTarget()
    {
        _Animator.SetFloat("Speed", 0);
        if (LocalTarget == null)
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
            if (LocalTarget.transform.position.x - transform.position.x != 0)
            {
                float dir = (LocalTarget.transform.position.x - transform.position.x) / Mathf.Abs(LocalTarget.transform.position.x - transform.position.x);
                _Animator.SetFloat("Speed", Mathf.Abs(dir));
                set_dir(dir);
                _rb.transform.Translate(new Vector3(Mathf.Abs(dir) * _Speed * Time.deltaTime, 0f, 0f));
            }
        }
    }

    protected void follow_GlobalTarget()
    {
        find_LocalTarget();
        _Animator.SetFloat("Speed", 0);
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
            float dir = 0f;       
            if (LocalTarget.transform.position.x - transform.position.x != 0)
            {
                dir = (LocalTarget.transform.position.x - transform.position.x) / Mathf.Abs(LocalTarget.transform.position.x - transform.position.x);
            }
            _Animator.SetFloat("Speed", Mathf.Abs(dir));
            set_dir(dir);
            _rb.transform.Translate(new Vector3(Mathf.Abs(dir) * _Speed * Time.deltaTime, 0f, 0f));
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
        List<Stairs> StairListCopy = new List<Stairs>(StairList);
        Stairs ClosestStair = null;
        if (dir == "up")
        {
            foreach (Stairs stairs in StairListCopy)
            {
                if (stairs.UpperStair == null)
                {
                    StairList.Remove(stairs);
                }
            }
        }
        else if (dir == "down")
        {
            foreach (Stairs stairs in StairListCopy)
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

    public void change_state(StateOfCharacter NewState, BaseType NewGlobalTarget = null)
    {
        CharacterState = NewState;
        GlobalTarget = NewGlobalTarget;
        LocalTarget = null;
        isGlobalTargetReached = false;

        _SpriteRender.sortingLayerName = _BaseSortingLayer;
        if (NewState == StateOfCharacter.Idle || NewState == StateOfCharacter.Working || 
            NewState == StateOfCharacter.StayInHall)
        {
            _SpriteRender.sortingLayerName = "InsideRoom";
        }
    }

    protected void reset_state()
    {
        GlobalTarget = null;
        LocalTarget = null;
        CharacterState = StateOfCharacter.Idle;
        _SpriteRender.sortingLayerName = "InsideRoom";
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

    private void set_dir(float dir)
    {
        if(dir > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }else if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            return;
        }
    }


}
