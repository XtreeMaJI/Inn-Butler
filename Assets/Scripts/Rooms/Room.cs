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

    protected void Awake()
    {
        configure_RoomType();
        _Can = transform.Find("Canvas").GetComponent<Canvas>();
        _HammerB = _Can.transform.Find("HammerB").GetComponent<Button>();
        _Can.worldCamera = Camera.main;
        _PC = null;
    }

    private void configure_RoomType()
    {
        if(this.GetComponent<Bedroom>())
        {
            RoomType = LevelManager.TypeOfRoom.Bedroom;
            return;
        }
        if (this.GetComponent<CheapRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.CheapRoom;
            return;
        }
        if (this.GetComponent<StandartRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.StandartRoom;
            return;
        }
        if (this.GetComponent<ComfortableRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.ComfortableRoom;
            return;
        }
        if (this.GetComponent<TraderRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.TraderRoom;
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

}
