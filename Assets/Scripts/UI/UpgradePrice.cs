using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradePrice : MonoBehaviour
{
    private TMP_Text _MoneyCount;
    private Color _ColorBuf;


    private void Start()
    {
        _MoneyCount = transform.Find("MoneyCount").GetComponent<TMP_Text>();
        _ColorBuf = _MoneyCount.color;
        string ParentName = transform.parent.name;

        switch(ParentName)
        {
            case "LevelDownButton":
                _MoneyCount.SetText("0");
                break;
            case "StairsButton":
                _MoneyCount.SetText(LevelManager.STAIRS_PRICE.ToString());
                break;
            case "StaffRoomButton":
                _MoneyCount.SetText(LevelManager.STAFF_ROOM_PRICE.ToString());
                break;
            case "HallButton":
                _MoneyCount.SetText(LevelManager.HALL_PRICE.ToString());
                break;
            case "KitchenButton":
                _MoneyCount.SetText(LevelManager.KITCHEN_PRICE.ToString());
                break;
            case "BarButton":
                _MoneyCount.SetText(LevelManager.BAR_PRICE.ToString());
                break;
        }
    }

    //Используется только для потомка LevelUpB
    //RoomType - тип комнаты, к которому на данный момент принадлежит комната 
    public void set_LevelUpButton_price(LevelManager.TypeOfRoom RoomType)
    {
        if(_MoneyCount == null)
        {
            Start();
        }

        switch(RoomType)
        {
            case LevelManager.TypeOfRoom.Room:
                _MoneyCount.SetText(LevelManager.BEDROOM_PRICE.ToString());
                break;
            case LevelManager.TypeOfRoom.Bedroom:
                _MoneyCount.SetText(LevelManager.CHEAP_ROOM_PRICE.ToString());
                break;
            case LevelManager.TypeOfRoom.CheapRoom:
                _MoneyCount.SetText(LevelManager.STANDRT_ROOM_PRICE.ToString());
                break;
            case LevelManager.TypeOfRoom.StandartRoom:
                _MoneyCount.SetText(LevelManager.COMFORTABLE_ROOM_PRICE.ToString());
                break;
            case LevelManager.TypeOfRoom.ComfortableRoom:
                _MoneyCount.SetText(LevelManager.TRADER_ROOM_PRICE.ToString());
                break;
            case LevelManager.TypeOfRoom.TraderRoom:
                _MoneyCount.SetText("0");
                break;
        }
    }

    public void to_twinkle_price_red()
    {
        _MoneyCount.color = Color.red;
        StartCoroutine("set_base_color");
    }

    private IEnumerator set_base_color()
    {
        yield return new WaitForSeconds(0.5f);
        _MoneyCount.color = _ColorBuf;
    }

    private void OnEnable()
    {
        if(_MoneyCount != null && _ColorBuf != null)
        {
            _MoneyCount.color = _ColorBuf;
        }
    }

}
