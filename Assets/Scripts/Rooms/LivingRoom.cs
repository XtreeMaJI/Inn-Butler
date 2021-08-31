using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingRoom : Room
{
    public enum StateOfLivingRoom : int
    {
        Empty = 0,
        VisitorInside = 1,
        AwaitForCleaning = 2,
        Cleaning = 3
    }

    private GameObject _InfoPanel;
    private Image _CleanBar;
    private Image _FoodBar;
    private Image _FunBar;

    private float _BaseCleanSpeed; //Скорость уборки, равная уборке спальной за час игрового времени
    private float _CleanSpeedMod; //Модификатор скорости уборки

    public StateOfLivingRoom RoomState { get; set; }

    private bool _IsFoodRequested = false; //Добавлена ли комната в очередь на еду
    private bool _IsWineRequested = false; //Добавлена ли комната в очередь на вино

    protected void Start()
    {
        _CheckInB = _Can.transform.Find("CheckInB").gameObject;
        _CleanB = _Can.transform.Find("CleanB").gameObject;
        _CancelCleanB = _Can.transform.Find("CancelCleanB").gameObject;
        _GiveItemB = _Can.transform.Find("GiveItemB").gameObject;

        init_InfoPanel();
        init_bars();

        RoomState = StateOfLivingRoom.Empty;

        _BaseCleanSpeed = LevelManager.BaseMaxClean / (LevelManager.DayLength / 24);
    }

    protected void Update()
    {
        decrease_bars();
        increase_Clean();
    }

    private void init_InfoPanel()
    {
        _InfoPanel = _Can.transform.Find("InfoPanel").gameObject;
        _CleanBar = _InfoPanel.transform.Find("CleanBar").GetComponent<Image>();
        _FoodBar = _InfoPanel.transform.Find("FoodBar").GetComponent<Image>();
        _FunBar = _InfoPanel.transform.Find("FunBar").GetComponent<Image>();
    }

    //Заселить посетителя
    public void press_CheckIn_button()
    {
        check_visitor_in(_PC.FollowingVisitor);
        _PC.FollowingVisitor = null;
    }

    public void check_visitor_in(Visitor NewVisitor)
    {
        NewVisitor.CurRoom = this;
        NewVisitor.change_state(BaseCharacter.StateOfCharacter.MoveToOwnRoom, this);
        Vis = NewVisitor;
        toggle_InfoPanel();
        disable_buttons();
        enable_buttons();
    }

    private void free_the_room()
    {
        Vis = null;
        RoomState = StateOfLivingRoom.Empty;
        _Scheduler.delete_room_from_CleanQueue(this);
        _Scheduler.delete_room_from_FoodQueue(this);
        _Scheduler.delete_room_from_WineQueue(this);
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _PC = collision.GetComponent<PlayerController>();
            enable_buttons();
        }
        else if (collision.tag == "Character")
        {
            collision.GetComponent<BaseCharacter>().RoomBuf = this;
        }
    }

    protected new void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _PC = null;
            disable_buttons();
        }
        else if (collision.tag == "Character")
        {
            collision.GetComponent<BaseCharacter>().RoomBuf = null;
        }
    }

    protected void init_bars()
    {
        Clean = MaxClean;
        Food = MaxFood;
        Fun = MaxFun;
    }

    protected void decrease_bars()
    {
        if (Vis != null)
        {
            if (RoomState == StateOfLivingRoom.VisitorInside && Clean >= 0f)
            {
                float OldClean = Clean;
                Clean -= (MaxClean / LevelManager.DayLength) * Time.deltaTime;
                _CleanBar.fillAmount = Clean / MaxClean;
                if(is_clean_dropped_by_half(OldClean) || Clean <= 0f)
                {
                    Vis.go_for_walk();
                    request_cleaning();
                    disable_buttons();
                    enable_buttons();
                }
            }

            if (RoomType != LevelManager.TypeOfRoom.Bedroom &&
               RoomType != LevelManager.TypeOfRoom.CheapRoom && 
               Food >= 0f)
            {
                Food -= (MaxFood / LevelManager.DayLength) * Time.deltaTime;
                _FoodBar.fillAmount = Food / MaxFood;
                if(_IsFoodRequested == false && 
                   MaxFood - Food >= LevelManager.BASE_AMOUNT_OF_FOOD_FOR_LIVING_ROOM / 2)
                {
                    request_food();
                }
            }

            if (RoomType == LevelManager.TypeOfRoom.TraderRoom && Fun >= 0f)
            {
                Fun -= (MaxFun / LevelManager.DayLength) * Time.deltaTime;
                _FunBar.fillAmount = Fun / MaxFun;
                if(_IsWineRequested == false && 
                   MaxFun - Fun >= LevelManager.BASE_AMOUNT_OF_WINE_FOR_LIVING_ROOM / 2)
                {
                    request_wine();
                }
            }
        }
    }

    private void toggle_InfoPanel()
    {
        if(_InfoPanel.activeSelf)
        {
            _InfoPanel.SetActive(false);
        }
        else
        {
            _InfoPanel.SetActive(true);
        }
    }

    public void press_CleanB()
    {
        if (_PC != null && RoomState == StateOfLivingRoom.Empty)
        {
            StartCleaning(LevelManager.PlayerCleanMod);
            _PC.set_PlayerState(PlayerController.StateOfPlayer.Cleaning);
            _CancelCleanB.SetActive(true);
            return;
        }
    }

    public void StartCleaning(float NewCleaningSpeedMod)
    {
        RoomState = StateOfLivingRoom.Cleaning;
        _CleanSpeedMod = NewCleaningSpeedMod;
    }

    public void StopCleaning()
    {
        RoomState = StateOfLivingRoom.Empty;
        _Scheduler.delete_room_from_CleanQueue(this);
        if (_PC != null)
        {
            _PC.set_PlayerState(PlayerController.StateOfPlayer.Common);
            _CancelCleanB.SetActive(false);
            disable_buttons();
            enable_buttons();
        }
    }

    private void increase_Clean()
    {
        if(RoomState == StateOfLivingRoom.Cleaning)
        {
            Clean += _CleanSpeedMod * _BaseCleanSpeed * Time.deltaTime;
            _CleanBar.fillAmount = Clean / MaxClean;
        }
        if(RoomState == StateOfLivingRoom.Cleaning && Clean >= MaxClean)
        {
            StopCleaning();
        }
    }

    public void press_GiveItemB()
    {
        _PC?.set_PlayerState(PlayerController.StateOfPlayer.Common);
        take_item_from_person(_PC?.get_Item());
        disable_buttons();
        enable_buttons();
    }

    public void take_item_from_person(Carryable item)
    {
        if(item.GetComponent<Food>())
        {
            refill_Food();
            Destroy(item.gameObject);
            return;
        }
        if (item.GetComponent<Wine>())
        {
            refill_Fun();
            Destroy(item.gameObject);
            return;
        }
    }

    public void refill_Food()
    {
        Food += LevelManager.AMOUNT_OF_FOOD_REFILLED_BY_DISH;
        _FoodBar.fillAmount = Food / MaxFood;
        _IsFoodRequested = false;
        _Scheduler.delete_room_from_FoodQueue(this);
    }

    public void refill_Fun()
    {
        Fun += LevelManager.AMOUNT_OF_FUN_REFILLED_BY_WINE;
        _FunBar.fillAmount = Fun / MaxFun;
        _IsWineRequested = false;
        _Scheduler.delete_room_from_WineQueue(this);
    }

    private void request_cleaning()
    {
        _Scheduler.add_room_in_CleanQueue(this);
    }

    private void request_food()
    {
        _Scheduler.add_room_in_FoodQueue(this);
        _IsFoodRequested = true;
    }

    private void request_wine()
    {
        _Scheduler.add_room_in_WineQueue(this);
        _IsWineRequested = true;
    }

    private bool is_clean_dropped_by_half(float OldCleanMes)
    {
        if(OldCleanMes >= MaxClean/2 && Clean < MaxClean/2)
        {
            return true;
        }
        return false;
    }

    protected override void enable_buttons()
    {
        if(_PC == null)
        {
            return;
        }

        if (_PC.get_PlayerState() == PlayerController.StateOfPlayer.Carrying &&
            RoomType != LevelManager.TypeOfRoom.Stairs)
        {
            _GiveItemB.SetActive(true);
            return;
        }

        if (_PC.FollowingVisitor != null && is_Suitable_for_TypeOfVisitor(_PC.FollowingVisitor.VisitorType) &&
            Vis == null)
        {
            _CheckInB.SetActive(true);
        }
        if (Clean < MaxClean && _PC.FollowingVisitor == null && Vis != null && RoomState == StateOfLivingRoom.Empty)
        {
            _CleanB.SetActive(true);
        }
        if (_PC.FollowingVisitor == null && Vis == null)
        {
            _HammerB.SetActive(true);
        }

    }

}
