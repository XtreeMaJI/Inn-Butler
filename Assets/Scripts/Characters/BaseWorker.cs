using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWorker : BaseCharacter
{
    protected LivingRoom _RoomForWork = null;
    protected float Speed = 1f;
    protected float WorkSpeedMod = 1f;
    protected int Salary = LevelManager.BASE_SALARY;

    protected void Start()
    {
        _MoneyManager.increase_TotalSalary(Salary);
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

}
