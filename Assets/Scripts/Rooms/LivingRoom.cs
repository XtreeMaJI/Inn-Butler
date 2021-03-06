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

    public LivingRoomInfoPanel InfoPanel;

    private float _BaseCleanSpeed; //Скорость уборки, равная уборке спальной за час игрового времени
    private float _CleanSpeedMod; //Модификатор скорости уборки

    public StateOfLivingRoom RoomState { get; private set; }

    private bool _IsFoodRequested = false; //Добавлена ли комната в очередь на еду
    private bool _IsWineRequested = false; //Добавлена ли комната в очередь на вино

    private GameObject _InnerWall;

    //Флаги того, что шкала, в ходе уменьшения, достигла нуля. После пополнения шкалы, флаги восстанавливаются в false
    private bool _IsFoodHitZero = false;
    private bool _IsCleanHitZero = false;
    private bool _IsFunHitZero = false;

    public float Happiness = 3f; //Уровень счастья
    public float HappinessPenalty = 1f; //Штраф по счастью, если шкала потребностей достигнет нуля
    public float HoursToDecreaseHappiness = 12f; //Количество часов, за которое счастье уменьшится на 1
    public float MaxHappiness = 5f;

    protected void Start()
    {
        _CheckInB = _Can.transform.Find("CheckInB").gameObject;
        _CleanB = _Can.transform.Find("CleanB").gameObject;
        _CancelCleanB = _Can.transform.Find("CancelCleanB").gameObject;
        _GiveItemB = _Can.transform.Find("GiveItemB").gameObject;

        _InnerWall = transform.Find("InnerWall").gameObject;

        init_bars();

        change_state(StateOfLivingRoom.Empty);

        _BaseCleanSpeed = LevelManager.BaseMaxClean / (LevelManager.DayLength / 24);
    }

    protected void Update()
    {
        decrease_bars();
        increase_Clean();
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
        InfoPanel.set_VisSprite(NewVisitor.GetComponent<Image>().sprite);
        InfoPanel.set_DaysBeforeLeave(Vis.NumOfDaysBeforeLeave);
        InfoPanel.set_HappinessBarFill(Happiness/MaxHappiness);
    }

    public void free_the_room()
    {
        Vis = null;
        toggle_InfoPanel();
        handle_visitor_leave_room();
        change_state(StateOfLivingRoom.Empty);
        _Scheduler.delete_room_from_FoodQueue(this);
        _Scheduler.delete_room_from_WineQueue(this);
        _IsFoodRequested = false;
        _IsWineRequested = false;
        Food = MaxFood;
        Fun = MaxFun;
        InfoPanel.set_VisSprite();
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
                InfoPanel.set_CleanBarFill(Clean / MaxClean);
            }

            if (RoomType != LevelManager.TypeOfRoom.Bedroom &&
               RoomType != LevelManager.TypeOfRoom.CheapRoom && 
               Food >= 0f)
            {
                Food -= (MaxFood / LevelManager.DayLength) * Time.deltaTime;
                InfoPanel.set_FoodBarFill(Food / MaxFood);
            }

            if (RoomType == LevelManager.TypeOfRoom.TraderRoom && Fun >= 0f)
            {
                Fun -= (MaxFun / LevelManager.DayLength) * Time.deltaTime;
                InfoPanel.set_FunBarFill(Fun / MaxFun);
            }

            if (_IsFoodRequested == false &&
                MaxFood - Food >= LevelManager.BASE_AMOUNT_OF_FOOD_FOR_LIVING_ROOM / 2)
            {
                request_food();
            }

            if (_IsWineRequested == false &&
                MaxFun - Fun >= LevelManager.BASE_AMOUNT_OF_WINE_FOR_LIVING_ROOM / 2)
            {
                request_wine();
            }

            //Если какая-то из шкал потребностей достигла нуля, то уменьшается шкала счастья
            if (Food <= 0f && _IsFoodHitZero == false)
            {
                _IsFoodHitZero = true;
                Happiness -= HappinessPenalty;
            }

            if (Clean <= 0f && _IsCleanHitZero == false)
            {
                _IsCleanHitZero = true;
                Happiness -= HappinessPenalty;
            }

            if (Fun <= 0f && _IsFunHitZero == false)
            {
                _IsFunHitZero = true;
                Happiness -= HappinessPenalty;
            }

            if (_IsFunHitZero || _IsFoodHitZero || _IsCleanHitZero)
            {
                Happiness -= (1f / HoursToDecreaseHappiness) * (24f / LevelManager.DayLength) * Time.deltaTime;
                InfoPanel.set_HappinessBarFill(Happiness / MaxHappiness);
            }

        }
    }

    private void toggle_InfoPanel()
    {
        if(InfoPanel.gameObject.activeSelf)
        {
            InfoPanel.gameObject.SetActive(false);
        }
        else
        {
            InfoPanel.gameObject.SetActive(true);
        }
    }

    public void press_CleanB()
    {
        if (_PC != null && RoomState == StateOfLivingRoom.Empty)
        {
            StartCleaning(LevelManager.PlayerCleanMod);
            _PC.set_PlayerState(PlayerController.StateOfPlayer.Cleaning);
            _CancelCleanB.SetActive(true);
            _PC.transform.SetPositionAndRotation(transform.position, new Quaternion());
            return;
        }
    }

    public void StartCleaning(float NewCleaningSpeedMod)
    {
        change_state(StateOfLivingRoom.Cleaning);
        _CleanSpeedMod = NewCleaningSpeedMod;
    }

    public void StopCleaning()
    {
        change_state(StateOfLivingRoom.Empty);
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
            InfoPanel.set_CleanBarFill(Clean / MaxClean);
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
        InfoPanel.set_FoodBarFill(Food / MaxFood);
        _IsFoodRequested = false;
        _Scheduler.delete_room_from_FoodQueue(this);
    }

    public void refill_Fun()
    {
        Fun += LevelManager.AMOUNT_OF_FUN_REFILLED_BY_WINE;
        InfoPanel.set_FunBarFill(Fun / MaxFun);
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

    //Применяется, когда посетитель выходит на прогулку или освобождает комнату насовсем
    public void handle_visitor_leave_room()
    {
        disable_buttons();
        enable_buttons();
        request_cleaning();
    }

    public void change_state(StateOfLivingRoom NewState)
    {
        RoomState = NewState;
        _InnerWall.SetActive(false);
        if(NewState == StateOfLivingRoom.VisitorInside)
        {
            _InnerWall.SetActive(true);
        }
    }

}
