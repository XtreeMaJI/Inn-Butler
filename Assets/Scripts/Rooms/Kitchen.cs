using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : BaseWorkerRoom, IRoomWithCarryables
{
    public Food FoodInst;
    public Wine WineInst;
    private Vector3 _PlaceForFood;

    private bool isFoodCooking;

    //Текущая и конечная степень готовки
    private float _CurCookDegree;
    private float _MaxCookDegree;

    private float _BaseCookSpeed;
    private float _CookSpeedMod;

    private void Start()
    {
        _CookFoodB = _Can.transform.Find("CookFoodB").gameObject;
        _PlaceForFood = transform.Find("PlaceForFood").position;
        _StopCookFoodB = _Can.transform.Find("StopCookFoodB").gameObject;
        isFoodCooking = false;

        _MaxCookDegree = 1f;
        _CurCookDegree = 0f;

        _BaseCookSpeed = _MaxCookDegree / (LevelManager.DayLength / 24);
        _BaseCookSpeed /= 9; //Базовая скорость готовки - 9 часов

        _FinishedDish = null;
    }

    private void Update()
    {
        CookFood();
    }

    public void CookFood()
    {
        if (isFoodCooking)
        {
            _CurCookDegree += _BaseCookSpeed * _CookSpeedMod * Time.deltaTime;
            if(_CurCookDegree >= _MaxCookDegree)
            {
                _FinishedDish = Instantiate(FoodInst, _PlaceForFood, new Quaternion());
                _FinishedDish.set_ParentRoom(this);
                _CurCookDegree = 0f;
                stop_cooking();
            }
        }
    }

    public void press_CookFoodB()
    {
        disable_buttons();
        _StopCookFoodB.SetActive(true);
        _PC?.set_PlayerState(PlayerController.StateOfPlayer.Cooking);
        start_cooking(LevelManager.PlayerCookMod);
    }

    public void press_StopCookFoodB()
    {
        stop_cooking();
    }

    public void start_cooking(float NewCookSpeedMod)
    {
        if(_FinishedDish != null)
        {
            return;
        }

        isFoodCooking = true;
        _CookSpeedMod = NewCookSpeedMod;
    }

    public void stop_cooking()
    {
        isFoodCooking = false;

        if(_PlayerBuf != null)
        {
            disable_buttons();
            _PC?.set_PlayerState(PlayerController.StateOfPlayer.Common);
            if (_FinishedDish == null)
            {
                enable_buttons();
            }
        }
    }

    //В данном случае параметр не используется, так как предмет в комнате только один
    public void delete_item_from_room(Carryable item)
    {
        _FinishedDish = null;
    }

}
