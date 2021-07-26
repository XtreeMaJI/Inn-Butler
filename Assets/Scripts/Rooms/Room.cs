using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    //Текущие параметры для комнаты
    public float Clean;
    public float Food;
    public float Fun;

    //Максимально возможные параметры для каждого типа комнат
    public float MaxClean;
    public float MaxFood;
    public float MaxFun;

    public Visitor Vis;

    private Canvas _Can;
    private Button _HammerB;

    private void Start()
    {
        _Can = transform.Find("Canvas").GetComponent<Canvas>();
        _HammerB = _Can.transform.Find("HammerB").GetComponent<Button>();

        _Can.worldCamera = Camera.main;
    }

    private void toggle_button()
    {
        if(_HammerB.gameObject.activeSelf)
        {
            _HammerB.gameObject.SetActive(false);
        }
        else
        {
            _HammerB.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            toggle_button();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            toggle_button();
        }
    }
}


