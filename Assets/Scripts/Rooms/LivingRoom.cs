using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingRoom : Room
{
    protected GameObject _CheckInB;

    private GameObject _InfoPanel;
    private Image _CleanBar;
    private Image _FoodBar;
    private Image _FunBar;

    protected void Start()
    {
        _CheckInB = _Can.transform.Find("CheckInB").gameObject;
        init_InfoPanel();
        init_bars();
    }

    protected void Update()
    {
        decrease_bars();
    }

    private void init_InfoPanel()
    {
        _InfoPanel = _Can.transform.Find("InfoPanel").gameObject;
        _CleanBar = _InfoPanel.transform.Find("CleanBar").GetComponent<Image>();
        _FoodBar = _InfoPanel.transform.Find("FoodBar").GetComponent<Image>();
        _FunBar = _InfoPanel.transform.Find("FunBar").GetComponent<Image>();
    }

    protected new void toggle_buttons()
    {
        if (_PC.FollowingVisitor != null && is_Suitable_for_TypeOfVisitor(_PC.FollowingVisitor.VisitorType))
        {
            if (_CheckInB.activeSelf)
            {
                _CheckInB.SetActive(false);
            }
            else
            {
                _CheckInB.SetActive(true);
            }
        }
        else
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
    }

    public void press_CheckIn_button()
    {
        _PC.FollowingVisitor.CurRoom = this;
        _PC.FollowingVisitor.change_state(BaseCharacter.StateOfCharacter.MoveToRoom, this);
        Vis = _PC.FollowingVisitor;
        toggle_InfoPanel();
        toggle_buttons();
        _PC.FollowingVisitor = null;
        toggle_buttons();
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _PC = collision.GetComponent<PlayerController>();
            toggle_buttons();
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
            toggle_buttons();
            _PC = null;
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
            Clean -= (MaxClean/LevelManager.DayLength)*Time.deltaTime;
            _CleanBar.fillAmount = Clean / MaxClean;
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

}
