using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : BaseType
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
    protected GameObject _HammerB;

    protected PlayerController _PC;

    public LevelManager.TypeOfRoom RoomType;
    public LevelManager.PosInRoomTable PosInTable;

    protected override void Awake()
    {
        base.Awake();
        init_Room();
        _Can = transform.Find("Canvas").GetComponent<Canvas>();
        if (RoomType != LevelManager.TypeOfRoom.Reception)
        {
            _HammerB = _Can.transform.Find("HammerB").gameObject;
        }
        _Can.worldCamera = Camera.main;
        _PC = null;
        Vis = null;
    } 

    private void init_Room()
    {
        if (this.GetComponent<Bedroom>())
        {
            RoomType = LevelManager.TypeOfRoom.Bedroom;
            MaxClean = 1f;
            return;
        }
        if (this.GetComponent<CheapRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.CheapRoom;
            MaxClean = 2f;
            return;
        }
        if (this.GetComponent<StandartRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.StandartRoom;
            MaxClean = 4f;
            MaxFood = 1f;
            return;
        }
        if (this.GetComponent<ComfortableRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.ComfortableRoom;
            MaxClean = 8f;
            MaxFood = 2f;
            return;
        }
        if (this.GetComponent<TraderRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.TraderRoom;
            MaxClean = 16f;
            MaxFood = 3f;
            MaxFun = 1f;
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
        if (this.GetComponent<Kitchen>())
        {
            RoomType = LevelManager.TypeOfRoom.Kitchen;
            return;
        }
        if (this.GetComponent<Reception>())
        {
            RoomType = LevelManager.TypeOfRoom.Reception;
            return;
        }
        RoomType = LevelManager.TypeOfRoom.Room;
    }

    protected void toggle_buttons()
    {
        if (_HammerB.activeSelf)
        {
            _HammerB.SetActive(false);
        }
        else
        {
            _HammerB.SetActive(true);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _PC = collision.GetComponent<PlayerController>();
            toggle_buttons();
        }
        else if (collision.tag == "Character")
        {
            collision.GetComponent<BaseCharacter>().RoomBuf = this;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            toggle_buttons();
            _PC = null;
        }
        else if (collision.tag == "Character")
        {
            collision.GetComponent<BaseCharacter>().RoomBuf = null;
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
