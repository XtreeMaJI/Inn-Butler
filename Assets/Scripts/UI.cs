using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private RoomBlock RoomBlockInst;
    public PlayerController Controller;

    public LevelManager LM;

    private GameObject _MainPanel;
    private GameObject _BuildPanel;

    public GameObject ContentPanel; //Скроллящийся фон на _BuildPanel

    public GameObject FirstAddB {private set; get;}
    public GameObject SecondAddB {private set; get;}
    public GameObject ThirdAddB { private set; get; }
    public GameObject FourthAddB { private set; get; }

    private void Awake()
    {
        _MainPanel = transform.Find("MainPanel").gameObject;
        _BuildPanel = transform.Find("BuildPanel").gameObject;

        ContentPanel = _BuildPanel.transform.Find("Content").gameObject;

        FirstAddB = ContentPanel.transform.Find("FirstAddButton").gameObject;
        SecondAddB = ContentPanel.transform.Find("SecondAddButton").gameObject;
        ThirdAddB = ContentPanel.transform.Find("ThirdAddButton").gameObject;
        FourthAddB = ContentPanel.transform.Find("FourthAddButton").gameObject;
    }

    //Открыть меню строительства новых комнат
    public void open_BuildPanel()
    {
        disable_all_panels();
        _BuildPanel.SetActive(true);
    }

    public void open_MainPanel()
    {
        disable_all_panels();
        _MainPanel.SetActive(true);
    }

    //Сделать неактивными все панели, привязанные к UI
    private void disable_all_panels()
    {
        _MainPanel.SetActive(false);
        _BuildPanel.SetActive(false);
    }

}
