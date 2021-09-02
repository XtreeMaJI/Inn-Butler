using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    public float CurrentHour = 0f;
    public float SecondsInGameHour;

    private LevelManager _LM;

    public delegate void new_day_Callback(); //Вызывается в начале каждого нового дня

    public new_day_Callback DayStarter;

    private void Awake()
    {
        SecondsInGameHour = LevelManager.DayLength / 24;
        _LM = Object.FindObjectOfType<LevelManager>();
        StartCoroutine("make_time_step");
    }

    //Увеличить текущее время на 1 час
    private IEnumerator make_time_step()
    {
        yield return new WaitForSeconds(SecondsInGameHour);
        CurrentHour++;
        if(CurrentHour == 6)
        {
            _LM.distribute_visitors();
        }
        if(CurrentHour == 24)
        {
            CurrentHour = 0;
            DayStarter();
        }
        StartCoroutine("make_time_step");
    }

}
