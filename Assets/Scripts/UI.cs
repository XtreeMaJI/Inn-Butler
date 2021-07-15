using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public PlayerController Controller;

    private GameObject _MainPanel;

    private Button _UpB;
    private Button _DownB;

    private void Start()
    {
        _MainPanel = transform.Find("MainPanel").gameObject;
        _UpB = _MainPanel.transform.Find("UpButton").GetComponent<Button>();
        _DownB = _MainPanel.transform.Find("DownButton").GetComponent<Button>();
    }

    //Показать/скрыть стрелки "вверх" и "вниз"
    public void toggle_arrows()
    {
        if(_UpB.gameObject.activeSelf)
        {
            _UpB.gameObject.SetActive(false);
            _DownB.gameObject.SetActive(false);
        }
        else
        {
            _UpB.gameObject.SetActive(true);
            _DownB.gameObject.SetActive(true);
        }
    }

    public void UpB_pressed()
    {
        Stairs stair = Controller.ActiveStair;
        if (stair != null)
        {
            stair.climb_up(Controller.gameObject);
        }
    }

    public void DownB_pressed()
    {
        Stairs stair = Controller.ActiveStair;
        if (stair != null)
        {
            stair.climb_down(Controller.gameObject);
        }
    }

}
