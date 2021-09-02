using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int CurrentMoney;
    private MoneyAmountUI _MoneyUI;
    private TimeCounter _TimeCounter;

    private int TotalRentPayment = 0; //Сколько игрок платит за аренду каждый день
    private int TotalSalaryPayment = 0; //Сколько игрок платит работникам каждый день

    private void Start()
    {
        _MoneyUI = Object.FindObjectOfType<MoneyAmountUI>();
        CurrentMoney = 0;
        increase_money(150);
        _TimeCounter = FindObjectOfType<TimeCounter>();
        _TimeCounter.DayStarter += handle_day_start;
    }

    public void increase_money(int NumOfMoney)
    {
        CurrentMoney += NumOfMoney;
        _MoneyUI.set_MoneyCount(CurrentMoney);
    }

    public void decrease_money(int NumOfMoney)
    {
        CurrentMoney -= NumOfMoney;
        _MoneyUI.set_MoneyCount(CurrentMoney);
    }

    public bool is_enough_money_for_purchase(int price)
    {
        if(CurrentMoney < price)
        {
            return false;
        }
        return true;
    }

    //true - если покупка состоялась, else - если нет
    public bool try_purchase(int price)
    {
        if(is_enough_money_for_purchase(price))
        {
            decrease_money(price);
            return true;
        }
        return false;
    }

    public void increase_TotalRent(int IncNum)
    {
        TotalRentPayment += IncNum;
    }

    public void decrease_TotalRent(int DecNum)
    {
        TotalRentPayment += DecNum;
    }

    public void increase_TotalSalary(int IncNum)
    {
        TotalSalaryPayment += IncNum;
    }

    public void decrease_TotalSalary(int DecNum)
    {
        TotalSalaryPayment += DecNum;
    }

    public void handle_day_start()
    {
        decrease_money(TotalSalaryPayment);
        decrease_money(TotalRentPayment);
    }

}
