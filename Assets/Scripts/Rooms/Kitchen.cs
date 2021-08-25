using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : BaseWorkerRoom
{
    public GameObject FoodInst;
    private Transform _PlaceForFood;

    private bool isFoodCooking;

    //Текущая и конечная степень готовки
    private float _CurCookDegree;
    private float _MaxCookDegree;

    private float _BaseCookSpeed;
    private float _CookSpeedMod;

    private GameObject _FinishedDish;

    private void Start()
    {
        _CookFoodB = _Can.transform.Find("CookFoodB").gameObject;
        _PlaceForFood = _Can.transform.Find("PlaceForFood");
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
            if(_CurCookDegree <= _MaxCookDegree)
            {
                _FinishedDish = Instantiate(FoodInst, _PlaceForFood);
                stop_cooking();
            }
        }
    }

    public void start_cooking(float NewCookSpeedMod)
    {
        isFoodCooking = false;
        _CookSpeedMod = NewCookSpeedMod;
    }

    public void stop_cooking()
    {
        isFoodCooking = false;
    }

    public void take_food()
    {
        _FinishedDish = null;
    }

}
