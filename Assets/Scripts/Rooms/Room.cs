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

    protected Canvas _Can;
    protected Button _HammerB;

    protected PlayerController _PC;

    public LevelManager.TypeOfRoom RoomType;
    public LevelManager.PosInRoomTable PosInTable;

    private GameObject InfoPanel;
    private Image CleanBar;
    private Image FoodBar;
    private Image FunBar;

    protected void Awake()
    {
        init_Room();
        _Can = transform.Find("Canvas").GetComponent<Canvas>();
        _HammerB = _Can.transform.Find("HammerB").GetComponent<Button>();
        _Can.worldCamera = Camera.main;
        _PC = null;
        Vis = null;
        init_InfoPanel();
    }

    private void init_InfoPanel()
    {
        if (RoomType == LevelManager.TypeOfRoom.Bedroom ||
            RoomType == LevelManager.TypeOfRoom.CheapRoom ||
            RoomType == LevelManager.TypeOfRoom.StandartRoom ||
            RoomType == LevelManager.TypeOfRoom.ComfortableRoom ||
            RoomType == LevelManager.TypeOfRoom.TraderRoom)
        {
            InfoPanel = _Can.transform.Find("InfoPanel").gameObject;
            CleanBar = InfoPanel.transform.Find("CleanBar").GetComponent<Image>();
            FoodBar = InfoPanel.transform.Find("FoodBar").GetComponent<Image>();
            FunBar = InfoPanel.transform.Find("FunBar").GetComponent<Image>();
        }
    }

    protected void decrease_bars()
    {
        if(Vis != null)
        {

        }
    }

    protected void Update()
    {
        
    }

    private void init_Room()
    {
        if (this.GetComponent<Bedroom>())
        {
            RoomType = LevelManager.TypeOfRoom.Bedroom;
            MaxClean = LevelManager.DayLength*0.1f;
            return;
        }
        if (this.GetComponent<CheapRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.CheapRoom;
            MaxClean = LevelManager.DayLength * 0.2f;
            return;
        }
        if (this.GetComponent<StandartRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.StandartRoom;
            MaxClean = LevelManager.DayLength * 0.4f;
            return;
        }
        if (this.GetComponent<ComfortableRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.ComfortableRoom;
            MaxClean = LevelManager.DayLength * 0.8f;
            return;
        }
        if (this.GetComponent<TraderRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.TraderRoom;
            MaxClean = LevelManager.DayLength * 1.6f;
            return;
        }
        if (this.GetComponent<StaffRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.StaffRoom;
            return;
        }
        if (this.GetComponent<Hall>())
        {
            RoomType = LevelManager.TypeOfRoom.Hall;
            return;
        }
        if (this.GetComponent<Stairs>())
        {
            RoomType = LevelManager.TypeOfRoom.Stairs;
            return;
        }
        RoomType = LevelManager.TypeOfRoom.Room;
    }

    protected void toggle_button()
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _PC = collision.GetComponent<PlayerController>();
            toggle_button();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _PC = null;
            toggle_button();
        }
    }

    public void press_upgrade_button()
    {
        if(_PC != null)
        {
            _PC.ui.open_UpgradePanel(this.gameObject);
        }
    }

    public bool is_Suitable_for_TypeOfVisitor(LevelManager.TypeOfVisitor VisitorType)
    {
        if (VisitorType == LevelManager.TypeOfVisitor.Traveller &&
            (RoomType == LevelManager.TypeOfRoom.Bedroom ||
             RoomType == LevelManager.TypeOfRoom.CheapRoom))
        {
            return true;
        }
        if (VisitorType == LevelManager.TypeOfVisitor.Citizen &&
            (RoomType == LevelManager.TypeOfRoom.CheapRoom ||
             RoomType == LevelManager.TypeOfRoom.StandartRoom ||
             RoomType == LevelManager.TypeOfRoom.ComfortableRoom))
        {
            return true;
        }
        if (VisitorType == LevelManager.TypeOfVisitor.Merchant &&
            (RoomType == LevelManager.TypeOfRoom.ComfortableRoom ||
             RoomType == LevelManager.TypeOfRoom.TraderRoom))
        {
            return true;
        }
        return false;
    }

}
