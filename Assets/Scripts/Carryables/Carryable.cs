using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//При создании обязательно вызывать set_ParentRoom
public class Carryable : MonoBehaviour, ICarryable
{
    private float TimeToReachCarryPlace = 1f;
    private Transform PlaceForCarry;

    private IRoomWithCarryables _ParentRoom;

    //private const float RANGE_TO_REACH_CARRY_POS = 0.01f;
    //private const float LOCAL_X_OF_CARRY_POS = 0f;

    //Переменные для нахождения траектории движения предмета
    //Коэффициенты параболы
    //private float a, b;
    //Координаты точек параболы
   // private float x1, y1, x2, y2, x3, y3;

    //float speed = 0f;

    public void grab(Transform NewPlaceForCarry)
    {
        PlaceForCarry = NewPlaceForCarry;
        this.transform.SetParent(PlaceForCarry);
        this.transform.localPosition = new Vector2(0f, 0f);
        delete_Carryable_from_room();
        //Анимация подлёта
        //StartCoroutine("start_moving_to_PlaceForCarry");
    }

    public void set_ParentRoom(IRoomWithCarryables NewParentRoom)
    {
        _ParentRoom = NewParentRoom;
    }

    private void delete_Carryable_from_room()
    {
        _ParentRoom.delete_item_from_room(this);
    }

/*
    IEnumerator start_moving_to_PlaceForCarry()
    {
        speed = Mathf.Abs(PlaceForCarry.position.x - transform.position.x) / TimeToReachCarryPlace;
        find_parabola_coefs();
        StartCoroutine("go_to_PlaceForCarry");
        yield return new WaitForFixedUpdate();
    }

    IEnumerator go_to_PlaceForCarry()
    {
        float x = transform.localPosition.x + speed * Time.deltaTime;
        float y = a * x *x + b * x;
        transform.localPosition = new Vector2(x, y);
        if (is_CarryableX_in_range(LOCAL_X_OF_CARRY_POS, RANGE_TO_REACH_CARRY_POS) == false)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine("go_to_PlaceForCarry");
        }
        yield return new WaitForFixedUpdate();
    }

    private void find_parabola_points()
    {
        x1 = transform.localPosition.x;
        y1 = transform.localPosition.y;
        x2 = LOCAL_X_OF_CARRY_POS;
        y2 = 0f;
        //0.1 - коэффициент отклонения предмета при подлёте к y = 0 в локальных координатах
        x3 = x1 - 0.1f*x1;
        y3 = Mathf.Abs(x1 - x2) * 2;
    }

    private void find_parabola_coefs()
    {
*//*        float DetSys, Det1, Det2, Det3;
        find_parabola_points();
        DetSys = x1 * x1 * (x2 - x3) - x1 * (x2 * x2 - x3 * x3) + (x2 * x2 * x3 - x3 * x3 * x2);
        Det1 = y1 * (x2 - x3) - x1 * (y2 - y3) + (y2 * x3 - y3 * x2);
        Det2 = x1 * x1 * (y2 - y3) - y1 * (x2 * x2 - x3 * x3) + (x2 * x2 * y3 - x3 * x3 * y2);
        Det3 = x1 * x1 * (x2 * y3 - x3 * y2) - x1 * (x2 * x2 * y3 - x3 * x3 * y2) + y1 * (x2 * x2 * x3 - x3 * x3 * x2);

        if(DetSys == 0f)
        {
            this.transform.SetPositionAndRotation(PlaceForCarry.position, new Quaternion());
            return;
        }*//*

        find_parabola_points();
        b = y1/(x1-x1*x3);
        a = -b*x3;
    }

    //Находится ли координата x предмета в окрестности точки
    private bool is_CarryableX_in_range(float x, float range)
    {
        if((transform.localPosition.x + range > x && transform.localPosition.x < x) ||
           (transform.localPosition.x - range < x && transform.localPosition.x > x))
        {
            return true;
        }
        return false;
    }*/

}
