using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingRoom : Room
{
    protected GameObject _CheckInB;

    private GameObject InfoPanel;
    private Image CleanBar;
    private Image FoodBar;
    private Image FunBar;

    protected void Start()
    {
        _CheckInB = _Can.transform.Find("CheckInB").gameObject;
        init_InfoPanel();
    }

    private void init_InfoPanel()
    {
        InfoPanel = _Can.transform.Find("InfoPanel").gameObject;
        CleanBar = InfoPanel.transform.Find("CleanBar").GetComponent<Image>();
        FoodBar = InfoPanel.transform.Find("FoodBar").GetComponent<Image>();
        FunBar = InfoPanel.transform.Find("FunBar").GetComponent<Image>();
    }

    protected new void toggle_buttons()
    {
        if (_PC.FollowingVisitor != null)
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

}
