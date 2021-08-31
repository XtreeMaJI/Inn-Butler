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
    protected GameObject _CleanB;
    protected GameObject _CheckInB; //Кнопка заселения посетителя в комнату(LivingRoom)
    protected GameObject _GiveItemB;
    protected GameObject _AddStaffB; //Кнопка добавления в персонала комнату 
    protected GameObject _CancelCleanB; //Кнопка отмена уборки(LivingRoom)
    protected GameObject _UpB;  //Кнопка вверх при заходе на лестницу
    protected GameObject _DownB;//Кнопка вниз при заходе на лестницу
    protected GameObject _TakeKeysB; //Кнопка сопровождения посетителя к комнате(Reception)
    protected GameObject _RejectB; //Кнопка отказа посетителю(Reception)
    protected GameObject _CookFoodB; //Кнопка готовки еды(Kitchen)
    protected GameObject _StopCookFoodB;

    protected PlayerController _PC;
    protected GameObject _PlayerBuf;

    protected Visitor _VisitorBuf;

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
            MaxClean = LevelManager.BaseMaxClean;
            return;
        }
        if (this.GetComponent<CheapRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.CheapRoom;
            MaxClean = 2f * LevelManager.BaseMaxClean;
            return;
        }
        if (this.GetComponent<StandartRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.StandartRoom;
            MaxClean = 4f * LevelManager.BaseMaxClean;
            MaxFood = LevelManager.BASE_AMOUNT_OF_FOOD_FOR_LIVING_ROOM;
            return;
        }
        if (this.GetComponent<ComfortableRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.ComfortableRoom;
            MaxClean = 8f * LevelManager.BaseMaxClean;
            MaxFood = 2 * LevelManager.BASE_AMOUNT_OF_FOOD_FOR_LIVING_ROOM;
            return;
        }
        if (this.GetComponent<TraderRoom>())
        {
            RoomType = LevelManager.TypeOfRoom.TraderRoom;
            MaxClean = 16f * LevelManager.BaseMaxClean;
            MaxFood = 3 * LevelManager.BASE_AMOUNT_OF_FOOD_FOR_LIVING_ROOM;
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
        if (this.GetComponent<Bar>())
        {
            RoomType = LevelManager.TypeOfRoom.Bar;
            return;
        }
        RoomType = LevelManager.TypeOfRoom.Room;
    }

    protected virtual void enable_buttons()
    {
        if(_PC?.get_PlayerState() == PlayerController.StateOfPlayer.Carrying &&
            RoomType != LevelManager.TypeOfRoom.Stairs)
        {
            _GiveItemB?.SetActive(true);
            return;
        }

        if (RoomType == LevelManager.TypeOfRoom.Room ||
            RoomType == LevelManager.TypeOfRoom.Hall)
        {
            _HammerB.SetActive(true);
            return;
        }

        if (RoomType == LevelManager.TypeOfRoom.Stairs)
        {
            _UpB.SetActive(true);
            _DownB.SetActive(true);
            _HammerB.SetActive(true);
            return;
        }

        if (RoomType == LevelManager.TypeOfRoom.Reception)
        {
            if (_PlayerBuf == null)
            {
                return;
            }
            _AddStaffB.SetActive(true);
            if (_VisitorBuf == null ||
              (_PlayerBuf != null && _PlayerBuf.GetComponent<PlayerController>().FollowingVisitor != null))
            {
                return;
            }
            if (_LM.is_suitable_room_exist(_VisitorBuf.VisitorType))
            {
                _TakeKeysB.SetActive(true);
            }
            else
            {
                _RejectB.SetActive(true);
            }
            return;
        }

        if (RoomType == LevelManager.TypeOfRoom.Stairs)
        {
            _UpB.SetActive(true);
            _DownB.SetActive(true);
            _HammerB.SetActive(true);
            return;
        }

        if (RoomType == LevelManager.TypeOfRoom.StaffRoom)
        {
            _HammerB.SetActive(true);
            _AddStaffB.SetActive(true);
            return;
        }
    }

    protected virtual void disable_buttons()
    {
        _HammerB?.SetActive(false);
        _GiveItemB?.SetActive(false);
        _UpB?.SetActive(false);
        _DownB?.SetActive(false);
        _RejectB?.SetActive(false);
        _TakeKeysB?.SetActive(false);
        _AddStaffB?.SetActive(false);
        _CookFoodB?.SetActive(false);
        _StopCookFoodB?.SetActive(false);
        _CleanB?.SetActive(false);
        _CheckInB?.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _PC = collision.GetComponent<PlayerController>();
            _PlayerBuf = collision.gameObject;
            enable_buttons();
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
            disable_buttons();
            _PC = null;
            _PlayerBuf = null;
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
