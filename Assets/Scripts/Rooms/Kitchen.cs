using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : BaseWorkerRoom, IRoomWithCarryables
{
    public enum StateOfKitchen: int
    {
        Waiting = 0,
        Cooking = 1,
    }

    public Food FoodInst;
    public Wine WineInst;
    private Vector3 _PlaceForFood;
    private Vector3 _PlaceForCook;

    //Текущая и конечная степень готовки
    private float _CurCookDegree;
    private float _MaxCookDegree;

    private float _BaseCookSpeed;
    private float _CookSpeedMod;

    private Cook _Cook = null;
    private Servant _Servant = null;

    StateOfKitchen RoomState = StateOfKitchen.Waiting;

    public Food FinishedDish = null;

    public KitchenInfoPanel InfoPanel;

    private void Start()
    {
        _CookFoodB = _Can.transform.Find("CookFoodB").gameObject;
        _PlaceForFood = transform.Find("PlaceForFood").position;
        _StopCookFoodB = _Can.transform.Find("StopCookFoodB").gameObject;

        _MaxCookDegree = 1f;
        _CurCookDegree = 0f;

        _BaseCookSpeed = _MaxCookDegree / (LevelManager.DayLength / 24);
        _BaseCookSpeed /= 9; //Базовая скорость готовки - 9 часов

        _PlaceForCook = transform.Find("PosForWork").position;
    }

    private void Update()
    {
        CookFood();
    }

    public void CookFood()
    {
        if (RoomState == StateOfKitchen.Cooking)
        {
            _CurCookDegree += _BaseCookSpeed * _CookSpeedMod * Time.deltaTime;
            if(_CurCookDegree >= _MaxCookDegree)
            {
                FinishedDish = Instantiate(FoodInst, _PlaceForFood, new Quaternion());
                FinishedDish.set_ParentRoom(this);
                _CurCookDegree = 0f;
                stop_cooking();
            }
            InfoPanel.set_FoodBar(_CurCookDegree);
        }
    }

    public void press_CookFoodB()
    {
        if(_PC == null)
        {
            return;
        }
        disable_buttons();
        _StopCookFoodB.SetActive(true);
        _PC.set_PlayerState(PlayerController.StateOfPlayer.Cooking);
        start_cooking(LevelManager.PlayerCookMod);
        _PC.transform.SetPositionAndRotation(_PlaceForCook, new Quaternion());
    }

    public void press_StopCookFoodB()
    {
        stop_cooking();
        _StopCookFoodB.SetActive(false);
    }

    public void start_cooking(float NewCookSpeedMod)
    {
        if(FinishedDish != null)
        {
            return;
        }

        RoomState = StateOfKitchen.Cooking;
        _CookSpeedMod = NewCookSpeedMod;
    }

    public void stop_cooking()
    {
        RoomState = StateOfKitchen.Waiting;

        if (_PlayerBuf != null)
        {
            disable_buttons();
            _PC?.set_PlayerState(PlayerController.StateOfPlayer.Common);
            if (FinishedDish == null)
            {
                enable_buttons();
            }
        }
    }

    //В данном случае параметр не используется, так как предмет в комнате только один
    public void delete_item_from_room(Carryable item)
    {
        FinishedDish = null;
    }

    public override bool is_worker_on_this_pos_exist(LevelManager.TypeOfWorker WorkerType)
    {
        if(WorkerType == LevelManager.TypeOfWorker.Cook && _Cook == null)
        {
            return false;
        }
        if (WorkerType == LevelManager.TypeOfWorker.Servant && _Servant == null)
        {
            return false;
        }
        return true;
    }

    public override void add_worker(BaseWorker NewWorker)
    {
        if (NewWorker == null)
        {
            return;
        }

        if (NewWorker.GetComponent<Cook>() != null &&
            _Cook == null)
        {
            _Cook = (NewWorker as Cook);
            InfoPanel.activate_CookImage();
        }
        if (NewWorker.GetComponent<Servant>() != null &&
            _Servant == null)
        {
            _Servant = (NewWorker as Servant);
            InfoPanel.activate_ServantImage();
        }
    }

    protected override void enable_buttons()
    {
        if (FinishedDish != null)
        {
            return;
        }
        _HammerB.SetActive(true);
        _AddStaffB.SetActive(true);

        if (_Cook == null)
        {
            _CookFoodB.SetActive(true);
        }
    }

}
