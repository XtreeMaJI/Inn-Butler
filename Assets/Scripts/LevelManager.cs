using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum TypeOfRoom: int
    {
        None = 0,
        Room = 1,
        Stairs = 2,
        Bedroom = 3,
        CheapRoom = 4,
        StandartRoom = 5,
        ComfortableRoom = 6,
        TraderRoom = 7,
        StaffRoom = 8,
        Hall = 9,
        Reception = 10,
        Kitchen = 11
    }

    public enum TypeOfVisitor : int
    {
        Traveller = 0,
        Citizen = 1,
        Merchant = 2
    }

    public struct PosInRoomTable
    {
        public int floor;
        public int NumOfRoom;
        public PosInRoomTable(int NewFloor, int NewNum)
        {
            floor = NewFloor;
            NumOfRoom = NewNum;
        }
    }

    //Высоту и ширину комнаты устанавливаем в CreateLevel
    public float RoomWidth;
    public float RoomHeight;

    //Константы
    //Отступы, смещения и размеры блоков комнат для BuildPanel
    public const float RoomBlockSize = 100;
    public const float RoomBlockShift = 10;
    public const int NumOfFloors = 4;
    public const int MaxRoomsOnFloor = 25; //Максимальное число комнат на этаже
    public const int SameTownTavernStartRep = 400;
    public const int SecondTownTavernStartRep = 6000;
    public const int ThirdTownTavernStartRep = 4000;
    public const int FourthTownTavernStartRep = 2000;
    public const int SameTownTavernMaxRep = 1200;
    public const int SecondTownTavernMaxRep = 12000;
    public const int ThirdTownTavernMaxRep = 7000;
    public const int FourthTownTavernMaxRep = 6000;
    public const float DayLength = 4f; //Длина дня в секундах
    public const int NumOfTaverns = 5;

    public UI ui;

    public Room RoomInst;
    public RoomBlock RoomBlockInst;
    public Stairs StairsInst;
    public Bedroom BedroomInst;
    public CheapRoom CheapRoomInst;
    public StandartRoom StandartRoomInst;
    public ComfortableRoom ComfortableRoomInst;
    public TraderRoom TraderRoomInst;
    public Kitchen KitchenInst;
    public Reception ReceptionInst;

    private int[,] RoomTable; 

    //Репутации таверн
    public int RepSelf;
    public int RepSameTown;
    public int RepSecondTown;
    public int RepThirdTown;
    public int RepFourthTown;

    private int _TotalNumOfVisitors = 100; //Общее ежедневное число путешественников
    private Vector3 _VistorSpawnPos = new Vector3(0f, 0f, 0f);

    public Traveller TravellerInst;
    public Citizen CitizenInst;
    public Merchant MerchantInst;

    private List<Room> _RoomList;

    private int _DailyAmountOfVisitors; //Максимальное количество посетителей, которые могут прийти за день
    public Visitor VisInQueue; //Посетитель у ресепшена

    private void Awake()
    {
        RoomTable = new int[NumOfFloors, MaxRoomsOnFloor];
        for(int i=0; i<NumOfFloors; i++)
        {
            for(int j=0; j<MaxRoomsOnFloor; j++)
            {
                RoomTable[i, j] = (int)TypeOfRoom.None;
            }
        }
        _RoomList = new List<Room>();
    }

    private void Start()
    {
        init_taverns();
        StartCoroutine("distribute_visitors");
    }

    //Добавить комнату на выбранный этаж
    public Room add_room(int floor)
    {
        GameObject AddButton;
        switch (floor)
        {
            case 0:
                AddButton = ui.FirstAddB;
                break;
            case 1:
                AddButton = ui.SecondAddB;
                break;
            case 2:
                AddButton = ui.ThirdAddB;
                break;
            case 3:
                AddButton = ui.FourthAddB;
                break;
            default:
                return null;
        }
        //Добавляем новую ячейку в ui на BuildPanel.Content
        Vector3 RoomBlockPos = AddButton.GetComponent<RectTransform>().localPosition;
        RoomBlock NewRB = Instantiate(RoomBlockInst, ui.ContentPanel.transform);
        NewRB.GetComponent<RectTransform>().localPosition = RoomBlockPos;
        RoomBlockPos.x += RoomBlockSize + RoomBlockShift;
        AddButton.GetComponent<RectTransform>().localPosition = RoomBlockPos;
        //Добавляем новую комнату на уровень
        int CurrentRoom = get_num_of_rooms_on_floor(floor);
        Vector3 RoomPos;
        RoomPos.x = CurrentRoom * RoomWidth;
        RoomPos.y = floor * RoomHeight;
        RoomPos.z = 0;
        Room NewRoom = Instantiate(RoomInst, RoomPos, new Quaternion());
        RoomTable[floor, CurrentRoom] = (int)NewRoom.RoomType;
        NewRoom.PosInTable = new PosInRoomTable(floor, CurrentRoom);
        //Если это первая комната на этаже, то сразу делаем её лестницей
        int NumOfStairsOnFloor = get_rooms_count_of_type_on_floor(TypeOfRoom.Stairs, floor);
        _RoomList.Add(NewRoom);
        if (NumOfStairsOnFloor == 0)
        {
            NewRoom = upgrade_room(NewRoom, TypeOfRoom.Stairs);
        }
        NewRoom.CurFloor = floor;
        return NewRoom;
    }

    void delete_room(Room SelectedRoom)
    {
        _RoomList.Remove(SelectedRoom);
        Destroy(SelectedRoom.gameObject);
    }

    //Получить количество комнат на выбранном этаже
    private int get_num_of_rooms_on_floor(int floor)
    {
        int NumOfRooms = 0;
        for(int i = 0; i<MaxRoomsOnFloor; i++)
        {
            if (RoomTable[floor, i] != (int)TypeOfRoom.None)
            {
                NumOfRooms += 1;
            }
        }
        return NumOfRooms;
    }

    //Улучшить выбранную комнату до выбранного уровня 
    public Room upgrade_room(Room SelectedRoom, TypeOfRoom Upgrade)
    {
        Vector3 Pos = SelectedRoom.transform.position;
        PosInRoomTable PosInTableBuf = SelectedRoom.PosInTable;
        Room RoomBuf = null;
        delete_room(SelectedRoom);
        switch (Upgrade)
        {
            case TypeOfRoom.Stairs:
                RoomBuf = Instantiate(StairsInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.Bedroom:
                RoomBuf = Instantiate(BedroomInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.Room:
                RoomBuf = Instantiate(RoomInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.CheapRoom:
                RoomBuf = Instantiate(CheapRoomInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.StandartRoom:
                RoomBuf = Instantiate(StandartRoomInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.ComfortableRoom:
                RoomBuf = Instantiate(ComfortableRoomInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.TraderRoom:
                RoomBuf = Instantiate(TraderRoomInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.Kitchen:
                RoomBuf = Instantiate(KitchenInst, Pos, new Quaternion());
                break;
            case TypeOfRoom.Reception:
                RoomBuf = Instantiate(ReceptionInst, Pos, new Quaternion());
                break;
        }
        RoomBuf.PosInTable = PosInTableBuf;
        RoomTable[PosInTableBuf.floor, PosInTableBuf.NumOfRoom] = (int)RoomBuf.RoomType;
        _RoomList.Add(RoomBuf);
        RoomBuf.CurFloor = PosInTableBuf.floor;
        return RoomBuf;
    }

    //Получить количество комнат определённого типа на этаже
    public int get_rooms_count_of_type_on_floor(TypeOfRoom RType, int floor)
    {
        int Result = 0;
        int NumOfRoomsOnFloor = get_num_of_rooms_on_floor(floor);
        for (int i=0; i < NumOfRoomsOnFloor; i++)
        {
            if(RoomTable[floor, i] == (int)RType)
            {
                Result++;
            }
        }
        return Result;
    }

    private void init_taverns()
    {
        RepSelf = 0;
        RepSameTown = SameTownTavernStartRep;
        RepSecondTown = SecondTownTavernStartRep;
        RepThirdTown = ThirdTownTavernStartRep;
        RepFourthTown = FourthTownTavernStartRep;
    }

    private void increace_TavernsRep()
    {
        RepSameTown += get_daily_RepIncome(SameTownTavernMaxRep, SameTownTavernStartRep, 100);
        RepSecondTown += get_daily_RepIncome(SecondTownTavernMaxRep, SecondTownTavernStartRep, 365);
        RepThirdTown += get_daily_RepIncome(ThirdTownTavernMaxRep, ThirdTownTavernStartRep, 150);
        RepFourthTown += get_daily_RepIncome(FourthTownTavernMaxRep, FourthTownTavernStartRep, 200);
    }

    private int get_daily_RepIncome(int MaxVal, int StartVal, int DaysToReach)
    {
        return (int)((MaxVal - StartVal)/DaysToReach);
    }

    //Определяем сколько посетителей должно появиться в нашей таверне, исходя из репутации
    IEnumerator distribute_visitors()
    {
        yield return new WaitForSeconds(DayLength); 
        increace_TavernsRep();
        _DailyAmountOfVisitors = _TotalNumOfVisitors * RepSelf / (RepSelf + RepSameTown + RepSecondTown + RepThirdTown + RepFourthTown);
        if(_DailyAmountOfVisitors < 3)
        {
            _DailyAmountOfVisitors = 3;
        }
        create_visitor_if_possible();
        StartCoroutine("distribute_visitors");
    }

    //Создаём посетителя в зависимости от количества комнат, для каждого типа посетителей
    public void create_visitor()
    {
        _DailyAmountOfVisitors--;
        int NumOfRoomsForTravellers = get_num_of_rooms_in_tavern_for_TypeOfVisitor(TypeOfVisitor.Traveller);
        int NumOfRoomsForCitizens = get_num_of_rooms_in_tavern_for_TypeOfVisitor(TypeOfVisitor.Citizen);
        int NumOfRoomsForMerchants = get_num_of_rooms_in_tavern_for_TypeOfVisitor(TypeOfVisitor.Merchant);
        int NumOfLivingRooms = NumOfRoomsForTravellers + NumOfRoomsForCitizens + NumOfRoomsForMerchants;
       
        float TravellerSpawnChance = 0f;
        float CitizenSpawnChance = 0f;
        float MerchantSpawnChance = 0f;

        if (NumOfLivingRooms != 0)
        {
            TravellerSpawnChance = NumOfRoomsForTravellers / NumOfLivingRooms;
            CitizenSpawnChance = NumOfRoomsForCitizens / NumOfLivingRooms;
            MerchantSpawnChance = NumOfRoomsForMerchants / NumOfLivingRooms;
        }

        float RandomNum = Random.Range(0f, 1f);
        if (RandomNum < TravellerSpawnChance)
        {
            VisInQueue = Instantiate(TravellerInst, _VistorSpawnPos, new Quaternion());
        }
        else if (RandomNum > TravellerSpawnChance &&
            RandomNum <= TravellerSpawnChance + CitizenSpawnChance)
        {
            VisInQueue = Instantiate(CitizenInst, _VistorSpawnPos, new Quaternion());
        }
        else if(RandomNum > TravellerSpawnChance + CitizenSpawnChance)
        {
            VisInQueue = Instantiate(MerchantInst, _VistorSpawnPos, new Quaternion());
        }
    }

    //Проверяем, выполняются ли условия для создания нового посетителя и создаём
    public void create_visitor_if_possible()
    {
        if(_DailyAmountOfVisitors > 0 && VisInQueue == null)
        {
            create_visitor();
        }
    }

    private int get_num_of_rooms_in_tavern_for_TypeOfVisitor(TypeOfVisitor VisitorType)
    {
        int NumOfSuitableRooms = 0;
        List<Room> RoomListBuf = new List<Room>(_RoomList);
        foreach (Room room in RoomListBuf)
        {
            if (room.is_Suitable_for_TypeOfVisitor(VisitorType))
            {
                NumOfSuitableRooms++;
            }
        }
        return NumOfSuitableRooms;
    }

    public List<Stairs> get_all_stairs_on_floor(int floor)
    {
        List<Stairs> StairList = new List<Stairs>();
        List<Room> RoomListBuf = new List<Room>(_RoomList);
        foreach (Room room in RoomListBuf)
        {
            if(room.CurFloor == floor && room.RoomType == TypeOfRoom.Stairs)
            {
                StairList.Add(room as Stairs);
            }
        }
        return StairList;
    }
}
