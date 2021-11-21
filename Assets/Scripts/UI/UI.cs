using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public PlayerController Controller;

    public LevelManager LM;
    private MoneyManager _MoneyManager;

    private GameObject _MainPanel;
    private GameObject _BuildPanel;
    private GameObject _UpgradePanel;
    private GameObject _AddStaffPanel;

    //Панели для разных комнат на StaffPanel
    private GameObject _AddStaffKitchenPanel;
    private GameObject _AddStaffStaffRoomPanel;
    private GameObject _AddStaffReceptionPanel;
    private GameObject _AddStaffBarPanel;

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
    private GameObject _BarB;

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
        _BarB = _UpgradePanel.transform.Find("BarButton").gameObject;

        _DestrWarn = _UpgradePanel.transform.Find("DestroyWarning").gameObject;

        _AddStaffPanel = transform.Find("AddStaffPanel").gameObject;
        _AddStaffKitchenPanel = _AddStaffPanel.transform.Find("KitchenPanel").gameObject;
        _AddStaffStaffRoomPanel = _AddStaffPanel.transform.Find("StaffRoomPanel").gameObject;
        _AddStaffReceptionPanel = _AddStaffPanel.transform.Find("ReceptionPanel").gameObject;
        _AddStaffBarPanel = _AddStaffPanel.transform.Find("BarPanel").gameObject;

        _RoomBuf = null;

        _MoneyManager = Object.FindObjectOfType<MoneyManager>();
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
        UpgradePrice Price;

        disable_upgrade_panel_buttons();
        _RoomBuf = SelectedRoom.GetComponent<Room>();

        if(_RoomBuf == null)
        {
            return;
        }

        if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Room)
        {
            Price = _LevelUpB.transform.Find("UpgradePrice").GetComponent<UpgradePrice>();
            Price.set_LevelUpButton_price(_RoomBuf.RoomType);
            _LevelUpB.SetActive(true);
            _StairsB.SetActive(true);
            _HallB.SetActive(true);
            _StaffRoomB.SetActive(true);
            _KitchenB.SetActive(true);
            _BarB.SetActive(true);
            return;
        }
        if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Stairs ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.Hall ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.StaffRoom ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.Kitchen ||
            _RoomBuf.RoomType == LevelManager.TypeOfRoom.Bar)
        {
            _DestroyUpgradesB.SetActive(true);
            return;
        }

        //Если комната относится к типу LivingRoom
        Price = _LevelUpB.transform.Find("UpgradePrice").GetComponent<UpgradePrice>();
        Price.set_LevelUpButton_price(_RoomBuf.RoomType);
        _LevelUpB.SetActive(true);
        _LevelDownB.SetActive(true);
        _DestroyUpgradesB.SetActive(true);
        _RoomBuf = SelectedRoom.GetComponent<Room>();
    }

    public void open_AddStaffPanel(BaseWorkerRoom CurrentRoom)
    {
        disable_all_panels();
        _RoomBuf = CurrentRoom;
        configure_AddStaffPanel(CurrentRoom.RoomType);
        _AddStaffPanel.SetActive(true);
    }

    private void configure_AddStaffPanel(LevelManager.TypeOfRoom CurrentRoomType)
    {
        disable_AddStaffPanel_panels();
        enable_AddStaffPanel_buttons();
        disable_buttons_with_existing_workers(CurrentRoomType);
        if (CurrentRoomType == LevelManager.TypeOfRoom.Kitchen)
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

        if (CurrentRoomType == LevelManager.TypeOfRoom.Bar)
        {
            _AddStaffBarPanel.SetActive(true);
            return;
        }
    }

    //Отключить на AddStaffPanel кнопку, если работник уже был нанят и включить шкалу опыта
    private void disable_buttons_with_existing_workers(LevelManager.TypeOfRoom RoomType)
    {
        BaseWorkerRoom BaseWorkerRoomBuf = _RoomBuf.GetComponent<BaseWorkerRoom>();
        switch (RoomType)
        {
            case LevelManager.TypeOfRoom.StaffRoom:
                if (BaseWorkerRoomBuf.is_worker_on_this_pos_exist(LevelManager.TypeOfWorker.Housemaid))
                {
                    _AddStaffStaffRoomPanel.transform.Find("HireHousemaidB").gameObject.SetActive(false);
                }
                break;
            case LevelManager.TypeOfRoom.Kitchen:
                if (BaseWorkerRoomBuf.is_worker_on_this_pos_exist(LevelManager.TypeOfWorker.Cook))
                {
                    _AddStaffKitchenPanel.transform.Find("HireCookB").gameObject.SetActive(false);
                }
                if (BaseWorkerRoomBuf.is_worker_on_this_pos_exist(LevelManager.TypeOfWorker.Servant))
                {
                    _AddStaffKitchenPanel.transform.Find("HireServantB").gameObject.SetActive(false);
                }
                break;
            case LevelManager.TypeOfRoom.Bar:
                if (BaseWorkerRoomBuf.is_worker_on_this_pos_exist(LevelManager.TypeOfWorker.Servant))
                {
                    _AddStaffBarPanel.transform.Find("HireServantB").gameObject.SetActive(false);
                }
                break;
            case LevelManager.TypeOfRoom.Reception:
                if (BaseWorkerRoomBuf.is_worker_on_this_pos_exist(LevelManager.TypeOfWorker.Servant))
                {
                    _AddStaffReceptionPanel.transform.Find("HireServantB").gameObject.SetActive(false);
                }
                break;
        }
    }

    public void handle_AddStaffPanel_button_press()
    {
        GameObject PressedButton = EventSystem.current.currentSelectedGameObject;
        HirePrice Price = null;
        if (PressedButton.transform.Find("HirePrice") != null)
        {
            Price = PressedButton.transform.Find("HirePrice").GetComponent<HirePrice>();
        }
        string NameOfPressedButton = PressedButton.name;
        if (_RoomBuf == null)
        {
            return;
        }
        switch (NameOfPressedButton)
        {
            case "HireHousemaidB":
                if(_MoneyManager.try_purchase(LevelManager.HOUSEMAID_PRICE))
                {
                    LM.create_worker(LevelManager.TypeOfWorker.Housemaid, _RoomBuf);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "HireCookB":
                if (_MoneyManager.try_purchase(LevelManager.COOK_PRICE))
                {
                    LM.create_worker(LevelManager.TypeOfWorker.Cook, _RoomBuf);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "HireServantB":
                if (_MoneyManager.try_purchase(LevelManager.SERVANT_PRICE))
                {
                    LM.create_worker(LevelManager.TypeOfWorker.Servant, _RoomBuf);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
        }
        configure_AddStaffPanel(_RoomBuf.RoomType);
    }

    //Обаботчик нажатия кнопок с панели апгрейдов
    public void handle_upgrade_panel_button_press()
    {
        GameObject PressedButton = EventSystem.current.currentSelectedGameObject;
        UpgradePrice Price = null;
        if (PressedButton.transform.Find("UpgradePrice") != null)
        {
            Price = PressedButton.transform.Find("UpgradePrice").GetComponent<UpgradePrice>();
        }
        string NameOfPressedButton = PressedButton.name;
        switch(NameOfPressedButton)
        {
            case "StairsButton":
                if(_MoneyManager.try_purchase(LevelManager.STAIRS_PRICE))
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Stairs);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "StaffRoomButton":
                if (_MoneyManager.try_purchase(LevelManager.STAFF_ROOM_PRICE))
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.StaffRoom);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "HallButton":
                if (_MoneyManager.try_purchase(LevelManager.HALL_PRICE))
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Hall);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "KitchenButton":
                if (_MoneyManager.try_purchase(LevelManager.KITCHEN_PRICE))
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Kitchen);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "BarButton":
                if (_MoneyManager.try_purchase(LevelManager.BAR_PRICE))
                {
                    _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Bar);
                }
                else
                {
                    Price.to_twinkle_price_red();
                    return;
                }
                break;
            case "DestroyUpgradesButton":
                //Если комната - это единственная лестница на этаже, то на даём удалить её
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
                    if (_MoneyManager.try_purchase(LevelManager.CHEAP_ROOM_PRICE))
                    {
                        _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.CheapRoom);
                    }
                    else
                    {
                        Price.to_twinkle_price_red();
                        return;
                    }
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.CheapRoom)
                {
                    if (_MoneyManager.try_purchase(LevelManager.STANDRT_ROOM_PRICE))
                    {
                        _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.StandartRoom);
                    }
                    else
                    {
                        Price.to_twinkle_price_red();
                        return;
                    }
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.StandartRoom)
                {
                    if (_MoneyManager.try_purchase(LevelManager.COMFORTABLE_ROOM_PRICE))
                    {
                        _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.ComfortableRoom);
                    }
                    else
                    {
                        Price.to_twinkle_price_red();
                        return;
                    }
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.ComfortableRoom)
                {
                    if (_MoneyManager.try_purchase(LevelManager.TRADER_ROOM_PRICE))
                    {
                        _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.TraderRoom);
                    }
                    else
                    {
                        Price.to_twinkle_price_red();
                        return;
                    }
                    break;
                }
                if (_RoomBuf.RoomType == LevelManager.TypeOfRoom.Room)
                {
                    if (_MoneyManager.try_purchase(LevelManager.BEDROOM_PRICE))
                    {
                        _RoomBuf = LM.upgrade_room(_RoomBuf, LevelManager.TypeOfRoom.Bedroom);
                    }
                    else
                    {
                        Price.to_twinkle_price_red();
                        return;
                    }
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
        _BarB.SetActive(false);
    }

    private void disable_AddStaffPanel_panels()
    {
        _AddStaffKitchenPanel.SetActive(false);
        _AddStaffStaffRoomPanel.SetActive(false);
        _AddStaffReceptionPanel.SetActive(false);
        _AddStaffBarPanel.SetActive(false);
    }

    private void enable_AddStaffPanel_buttons()
    {
        _AddStaffStaffRoomPanel.transform.Find("HireHousemaidB").gameObject.SetActive(true);
        _AddStaffKitchenPanel.transform.Find("HireCookB").gameObject.SetActive(true);
        _AddStaffKitchenPanel.transform.Find("HireServantB").gameObject.SetActive(true);
        _AddStaffBarPanel.transform.Find("HireServantB").gameObject.SetActive(true);
        _AddStaffReceptionPanel.transform.Find("HireServantB").gameObject.SetActive(true);
    }

    //Сделать неактивными все панели, привязанные к UI
    private void disable_all_panels()
    {
        _MainPanel.SetActive(false);
        _BuildPanel.SetActive(false);
        _UpgradePanel.SetActive(false);
        _AddStaffPanel.SetActive(false);
    }

    public void handle_build_panel_button_press(AddRoomButton AddButton)
    {
        if(_MoneyManager.is_enough_money_for_purchase(AddButton.Price.get_Price()))
        {
            LM.add_room(AddButton.Floor);
            AddButton.check_num_of_rooms();
            _MoneyManager.decrease_money(AddButton.Price.get_Price());
            _MoneyManager.increase_TotalRent(AddButton.Price.get_Rent());
            AddButton.Price.increase_rent();
        } 
        else
        {
            AddButton.Price.to_twinkle_price_red();
        }
    }

    IEnumerator show_DestrWarn()
    {
        _DestrWarn.SetActive(true);
        yield return new WaitForSeconds(3f);
        _DestrWarn.SetActive(false);
    }

}
