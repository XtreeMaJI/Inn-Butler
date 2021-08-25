using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public PlayerController Controller;

    public LevelManager LM;

    private GameObject _MainPanel;
    private GameObject _BuildPanel;
    private GameObject _UpgradePanel;
    private GameObject _AddStaffPanel;

    //Панели для разных комнат на StaffPanel
    private GameObject _AddStaffKitchenPanel;
    private GameObject _AddStaffStaffRoomPanel;
    private GameObject _AddStaffReceptionPanel;

    public GameObject ContentPanel; //Скроллящийся фон на _BuildPanel

    public GameObject FirstAddB {private set; get;}
    public GameObject SecondAddB {private set; get;}
    public GameObject ThirdAddB { private set; get; }
    public GameObject FourthAddB { private set; get; }

    //Кнопки на панели апгрейдов
    private GameObject _LevelUpB;
    private GameObject _LevelDownB;
    private GameObject _StairsB;
    private GameObject _StaffRoomB;
    private GameObject _HallB;
    private GameObject _KitchenB;
    private GameObject _DestroyUpgradesB;

    //Сообщение о невозможности удаления единственной лестницы
    private GameObject _DestrWarn;

    private Room _RoomBuf; //Буфер для комнаты, с которой взаимодействуем

    private void Awake()
    {
        _MainPanel = transform.Find("MainPanel").gameObject;
        _BuildPanel = transform.Find("BuildPanel").gameObject;
        _UpgradePanel = transform.Find("UpgradePanel").gameObject;
        ContentPanel = _BuildPanel.transform.Find("Content").gameObject;

        FirstAddB = ContentPanel.transform.Find("FirstAddButton").gameObject;
        SecondAddB = ContentPanel.transform.Find("SecondAddButton").gameObject;
        ThirdAddB = ContentPanel.transform.Find("ThirdAddButton").gameObject;
        FourthAddB = ContentPanel.transform.Find("FourthAddButton").gameObject;

        _LevelUpB = _UpgradePanel.transform.Find("LevelUpButton").gameObject;
        _LevelDownB = _UpgradePanel.transform.Find("LevelDownButton").gameObject;
        _StairsB = _UpgradePanel.transform.Find("StairsButton").gameObject;
        _StaffRoomB = _UpgradePanel.transform.Find("StaffRoomButton").gameObject;
        _HallB = _UpgradePanel.transform.Find("HallButton").gameObject;
        _KitchenB = _UpgradePanel.transform.Find("KitchenButton").gameObject;
        _DestroyUpgradesB = _UpgradePanel.transform.Find("DestroyUpgradesButton").gameObject;

        _DestrWarn = _UpgradePanel.transform.Find("DestroyWarning").gameObject;

        _AddStaffPanel = transform.Find("AddStaffPanel").gameObject;
        _AddStaffKitchenPanel = _AddStaffPanel.transform.Find("KitchenPanel").gameObject;
        _AddStaffStaffRoomPanel = _AddStaffPanel.transform.Find("StaffRoomPanel").gameObject;
        _AddStaffReceptionPanel = _AddStaffPanel.transform.Find("ReceptionPanel").gameObject;

        _RoomBuf = null;
    }

    //Открыть меню строительства новых комнат
    public void open_BuildPanel()
    {
        disable_all_panels();
        _BuildPanel.SetActive(true);
    }

    public void open_MainPanel()
    {
        disable_all_panels();
        _MainPanel.SetActive(true);
    }

    public void open_UpgradePanel(GameObject SelectedRoom)
    {
        disable_all_panels();
        configure_UpgradePanel(SelectedRoom);
        _UpgradePanel.SetActive(true);
    }

    //Настраиваем, какие кнопки будут показаны
    public void configure_UpgradePanel(GameObject SelectedRoom)
    {
        disable_upgrade_panel_buttons();
        _RoomBuf = SelectedRoom.GetComponent<Room>();

        if(_RoomBuf == null)
        {
            return;
        }

        if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Room)
        {
            _LevelUpB.SetActive(true);
            _StairsB.SetActive(true);
            _HallB.SetActive(true);
            _StaffRoomB.SetActive(true);
            _KitchenB.SetActive(true);
            return;
        }
        if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Stairs ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.Hall ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.StaffRoom ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.Kitchen)
        {
            _DestroyUpgradesB.SetActive(true);
            return;
        }

        //Если комната относится к типу LivingRoom
        _LevelUpB.SetActive(true);
        _LevelDownB.SetActive(true);
        _DestroyUpgradesB.SetActive(true);
        _RoomBuf = SelectedRoom.GetComponent<Room>();
    }

    public void open_AddStaffPanel(LevelManager.TypeOfRoom CurrentRoomType)
    {
        disable_all_panels();
        configure_AddStaffPanel(CurrentRoomType);
        _AddStaffPanel.SetActive(true);
    }

    private void configure_AddStaffPanel(LevelManager.TypeOfRoom CurrentRoomType)
    {
        disable_AddStaffPanel_buttons();
        if(CurrentRoomType == LevelManager.TypeOfRoom.Kitchen)
        {
            _AddStaffKitchenPanel.SetActive(true);
            return;
        }

        if (CurrentRoomType == LevelManager.TypeOfRoom.StaffRoom)
        {
            _AddStaffStaffRoomPanel.SetActive(true);
            return;
        }

        if (CurrentRoomType == LevelManager.TypeOfRoom.Reception)
        {
            _AddStaffReceptionPanel.SetActive(true);
            return;
        }
    }

    //Обаботчик нажатия кнопок с панели апгрейдов
    public void handle_upgrade_panel_button_press()
    {
        string NameOfPressedButton = EventSystem.current.currentSelectedGameObject.name;
        switch(NameOfPressedButton)
        {
            case "StairsButton":
                _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Stairs);
                break;
            case "StaffRoomButton":
                _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.StaffRoom);
                break;
            case "HallButton":
                _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Hall);
                break;
            case "KitchenButton":
                _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Kitchen);
                break;
            case "DestroyUpgradesButton":
                //Если команта - это единственная лестница на этаже, то на даём удалить её
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Stairs &&
                   LM.get_rooms_count_of_type_on_floor(LevelManager.TypeOfRoom.Stairs, _RoomBuf.PosInTable.floor) == 1)
                {
                    StartCoroutine("show_DestrWarn");
                    break;
                }
                _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Room);
                break;
            case "LevelUpButton":
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Bedroom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.CheapRoom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.CheapRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.StandartRoom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.StandartRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.ComfortableRoom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.ComfortableRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.TraderRoom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Room)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Bedroom);
                    break;
                }
                break;
            case "LevelDownButton":
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Bedroom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Room);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.CheapRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Bedroom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.StandartRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.CheapRoom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.ComfortableRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.StandartRoom);
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.TraderRoom)
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.ComfortableRoom);
                    break;
                }
                break;
        }
        configure_UpgradePanel(_RoomBuf.gameObject);
    }

    //Сделать неактивными все кнопки на панели апгрейдов
    private void disable_upgrade_panel_buttons()
    {
        _LevelUpB.SetActive(false);
        _LevelDownB.SetActive(false);
        _StaffRoomB.SetActive(false);
        _StairsB.SetActive(false);
        _HallB.SetActive(false);
        _KitchenB.SetActive(false);
        _DestroyUpgradesB.SetActive(false);
    }

    private void disable_AddStaffPanel_buttons()
    {
        _AddStaffKitchenPanel.SetActive(false);
        _AddStaffStaffRoomPanel.SetActive(false);
        _AddStaffReceptionPanel.SetActive(false);
    }

    //Сделать неактивными все панели, привязанные к UI
    private void disable_all_panels()
    {
        _MainPanel.SetActive(false);
        _BuildPanel.SetActive(false);
        _UpgradePanel.SetActive(false);
        _AddStaffPanel.SetActive(false);
    }

    public void handle_build_panel_button_press(int floor)
    {
        LM.add_room(floor);
    }

    IEnumerator show_DestrWarn()
    {
        _DestrWarn.SetActive(true);
        yield return new WaitForSeconds(3f);
        _DestrWarn.SetActive(false);
    }

}
