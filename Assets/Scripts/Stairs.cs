using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Stairs : Room
{
    //Лестницы, соединённые с текущей
    public Stairs UpperStair;
    public Stairs LowerStair;

    public Vector3 MidPos; //Середина лестницы

    private void Start()
    {
        _UpB = _Can.transform.Find("ArrowUp").gameObject;
        _DownB = _Can.transform.Find("ArrowDown").gameObject;

        UpperStair = null;
        LowerStair = null;

        _PlayerBuf = null;

        MidPos = transform.position;

        connect_stairs();
    }

    //Проверяем, есть ли сверху и снизу другие лестницы и соединяем, если есть 
    void connect_stairs()
    {
        if (CurFloor != 0)
        {
            List<Stairs> ListOfDownStairs = _LM.get_all_stairs_on_floor(CurFloor - 1);
            foreach (Stairs stair in ListOfDownStairs)
            {
                if (stair.PosInTable.NumOfRoom == PosInTable.NumOfRoom)
                {
                    stair.UpperStair = this;
                    LowerStair = stair;
                }
            }
        }
        if(CurFloor < LevelManager.NumOfFloors - 1)
        {
            List<Stairs> ListOfTopStairs = _LM.get_all_stairs_on_floor(CurFloor + 1);
            foreach (Stairs stair in ListOfTopStairs)
            {
                if (stair.PosInTable.NumOfRoom == PosInTable.NumOfRoom)
                {
                    stair.LowerStair = this;
                    UpperStair = stair;
                }
            }
        }
    }

    public void press_arrow_button()
    {
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        if(ButtonName == "ArrowUp")
        {
            climb_up(_PlayerBuf);
        }
        else
        {
            climb_down(_PlayerBuf);
        }
    }

    public void climb_up(GameObject Person)
    {
        if (UpperStair != null)
        {
            Person.transform.SetPositionAndRotation(UpperStair.MidPos, new Quaternion());
            Person.GetComponent<BaseType>().CurFloor = UpperStair.CurFloor;
            if (Person.tag == "Player")
            {
                _PlayerBuf = null;
                _PC = null;
            }
            else if (Person.tag == "Character")
            {
                Person.GetComponent<BaseCharacter>().RoomBuf = UpperStair;
            }
        }
    }

    public void climb_down(GameObject Person)
    {
        if (LowerStair != null)
        {
            Person.transform.SetPositionAndRotation(LowerStair.MidPos, new Quaternion());
            Person.GetComponent<BaseType>().CurFloor = LowerStair.CurFloor;
            if (Person.tag == "Player")
            {
                _PlayerBuf = null;
                _PC = null;
            }
            else if (Person.tag == "Character")
            {
                Person.GetComponent<BaseCharacter>().RoomBuf = LowerStair;
            }
        }
    }

}
