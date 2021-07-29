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

    private LevelManager LM;

    private GameObject _UpB;
    private GameObject _DownB;

    private GameObject _PlayerBuf; //Заносим сюда персонажа игрока, пока он стоит в проходе

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
        Stairs[] ListOfStairs = FindObjectsOfType<Stairs>();
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        foreach (Stairs stair in ListOfStairs)
        {
            if(stair.transform.position.x == x)
            {
                if(stair.transform.position.y > y)
                {
                    UpperStair = stair;
                    stair.LowerStair = this.GetComponent<Stairs>();
                }
                else if(stair.transform.position.y < y)
                {
                    LowerStair = stair;
                    stair.UpperStair = this.GetComponent<Stairs>();
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
            if (Person.tag == "Player")
            {
                _PlayerBuf = null;
                _PC = null;
            }
        }
    }

    public void climb_down(GameObject Person)
    {
        if (LowerStair != null)
        {
            Person.transform.SetPositionAndRotation(LowerStair.MidPos, new Quaternion());
            if (Person.tag == "Player")
            {
                _PlayerBuf = null;
                _PC = null;
            }
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _PlayerBuf = collision.gameObject;
            _PC = collision.GetComponent<PlayerController>();
            toggle_buttons();
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _PlayerBuf = null;
            _PC = null;
            toggle_buttons();
        }
    }

    private void toggle_buttons()
    {
        if(_UpB.activeSelf)
        {
            _UpB.SetActive(false);
            _DownB.SetActive(false);
            _HammerB.gameObject.SetActive(false);
        }
        else
        {
            _UpB.SetActive(true);
            _DownB.SetActive(true);
            _HammerB.gameObject.SetActive(true);
        }
    }

}
