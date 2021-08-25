using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingRoom : Room
{
    private GameObject _InfoPanel;
    private Image _CleanBar;
    private Image _FoodBar;
    private Image _FunBar;

    private bool _IsCleanGoing; //Идёт ли уборка

    private float _BaseCleanSpeed; //Скорость уборки, равная уборке спальной за час игрового времени
    private float _CleanSpeedMod; //Модификатор скорости уборки


    protected void Start()
    {
        _CheckInB = _Can.transform.Find("CheckInB").gameObject;
        _CleanB = _Can.transform.Find("CleanB").gameObject;
        _CancelCleanB = _Can.transform.Find("CancelCleanB").gameObject;

        init_InfoPanel();
        init_bars();

        _IsCleanGoing = false;

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
        _PC.FollowingVisitor.CurRoom = this;
        _PC.FollowingVisitor.change_state(BaseCharacter.StateOfCharacter.MoveToRoom, this);
        Vis = _PC.FollowingVisitor;
        toggle_InfoPanel();
        disable_buttons();
        _PC.FollowingVisitor = null;
        enable_buttons();
    }

    private void free_the_room()
    {
        Vis = null;
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
            if (_IsCleanGoing == false)
            {
                Clean -= (MaxClean / LevelManager.DayLength) * Time.deltaTime;
                _CleanBar.fillAmount = Clean / MaxClean;
            }

            if (RoomType != LevelManager.TypeOfRoom.Bedroom &&
               RoomType != LevelManager.TypeOfRoom.CheapRoom)
            {
                Food -= (MaxFood / LevelManager.DayLength) * Time.deltaTime;
                _FoodBar.fillAmount = Food / MaxFood;
            }

            if (RoomType == LevelManager.TypeOfRoom.TraderRoom)
            {
                Fun -= (MaxFun / LevelManager.DayLength) * Time.deltaTime;
                _FunBar.fillAmount = Fun / MaxFun;
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
        if (_PC != null && _IsCleanGoing == false)
        {
            StartCleaning(LevelManager.PlayerCleanMod);
            _PC.set_PlayerState(PlayerController.StateOfPlayer.Cleaning);
            _CancelCleanB.SetActive(true);
            return;
        }
    }

    private void StartCleaning(float NewCleaningSpeedMod)
    {
        _CleanSpeedMod = NewCleaningSpeedMod;
        _IsCleanGoing = true;
    }

    public void StopCleaning()
    {
        _IsCleanGoing = false;
        if(_PC != null)
        {
            _PC.set_PlayerState(PlayerController.StateOfPlayer.Common);
            _CancelCleanB.SetActive(false);
        }
    }

    private void increase_Clean()
    {
        if(_IsCleanGoing)
        {
            Clean += _CleanSpeedMod * _BaseCleanSpeed * Time.deltaTime;
            _CleanBar.fillAmount = Clean / MaxClean;
        }
        if(Clean >= MaxClean)
        {
            StopCleaning();
        }
    }

}
