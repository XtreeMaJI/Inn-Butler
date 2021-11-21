using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWorker : BaseCharacter
{
    protected LivingRoom _RoomForWork = null;
    protected float Speed = 1f;
    protected float WorkSpeedMod = 0.5f;
    protected int Salary = LevelManager.BASE_SALARY;
    protected Vector3 PlaceInWorkerRoom; 
    protected Quaternion DirInWorkerRoom = new Quaternion(0, 180, 0, 1); //Направление взгляда в собственной комнате 

    protected int CurXP = 0;
    protected int[] XPForLevel = {0, 10, 50, 100 };
    protected int Level = 0;

    protected void Start()
    {
        _MoneyManager.increase_TotalSalary(Salary);
        change_XP_on_UI();
    }

    private void Update()
    {
        switch (CharacterState)
        {
            case StateOfCharacter.Idle:
                find_work();
                break;
            case StateOfCharacter.MoveToWorkerRoom:
                move_to_GlobalTarget();
                if (isGlobalTargetReached == true)
                {
                    transform.SetPositionAndRotation(PlaceInWorkerRoom, DirInWorkerRoom);
                    reset_state();
                }
                break;
            case StateOfCharacter.MoveToPlaceForWork:
                move_to_GlobalTarget();
                if (isGlobalTargetReached == true)
                {
                    change_state(StateOfCharacter.Working);
                }
                break;
            case StateOfCharacter.Working:
                do_work();
                break;
        }
    }

    protected abstract void do_work();

    protected abstract void find_work();

    public abstract void set_WorkerRoom(BaseWorkerRoom RoomForWorker);

    public float get_WorkSpeedMod()
    {
        return WorkSpeedMod;
    }

    protected void OnDestroy()
    {
        _MoneyManager.decrease_TotalSalary(Salary);
    }

    protected virtual void increase_XP()
    {
        CurXP++;
        increace_level_if_needed();
        change_XP_on_UI();
    }

    protected abstract void change_XP_on_UI();

    protected virtual void increace_level_if_needed()
    {
        if(Level+1 >= XPForLevel.Length)
        {
            return;
        }

        if(XPForLevel[Level+1] <= CurXP)
        {
            Speed += 0.5f;
            WorkSpeedMod *= 2;
            Level++;
        }
        
    }

}
