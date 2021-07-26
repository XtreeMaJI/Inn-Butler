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

    private GameObject _Can;

    private GameObject _PlayerBuf; //Заносим сюда персонажа игрока, пока он стоит в проходе

    private void Start()
    {
        _Can = transform.Find("Canvas").gameObject;
        _UpB = _Can.transform.Find("ArrowUp").gameObject;
        _DownB = _Can.transform.Find("ArrowDown").gameObject;

        _Can.GetComponent<Canvas>().worldCamera = Camera.main;

        UpperStair = null;
        LowerStair = null;

        _PlayerBuf = null;

        MidPos = transform.position;

        connect_stairs();
    }

    public void init(LevelManager NewLM)
    {
        LM = NewLM;
        MidPos.y -= LM.RoomHeight / 2;
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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _PlayerBuf = collision.gameObject;
            toggle_buttons();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _PlayerBuf = null;
            toggle_buttons();
        }
    }

    private void toggle_buttons()
    {
        if(_UpB.activeSelf)
        {
            _UpB.SetActive(false);
            _DownB.SetActive(false);
        }
        else
        {
            _UpB.SetActive(true);
            _DownB.SetActive(true);
        }
    }

}
