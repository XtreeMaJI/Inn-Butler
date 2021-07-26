using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Высоту и ширину комнаты устанавливаем в CreateLevel
    public float RoomWidth;
    public float RoomHeight;

    //Отступы, смещения и размеры блоков комнат для BuildPanel
    public const float RoomBlockSize = 100;
    public const float RoomBlockShift = 10;

    public const int NumOfFloors = 4;
    public const int MaxRoomsOnFloor = 25; //Максимальное число комнат на этаже

    public UI ui;

    public Room RoomInst;
    public RoomBlock RoomBlockInst;

    private int[,] RoomTable;

    private void Awake()
    {
        RoomTable = new int[NumOfFloors, MaxRoomsOnFloor];
        for(int i=0; i<NumOfFloors; i++)
        {
            for(int j=0; j<MaxRoomsOnFloor; j++)
            {
                RoomTable[i, j] = 0;
            }
        }
    }

    //Добавить комнату на выбранный этаж
    public void add_room(int floor)
    {
        GameObject AddButton;
        switch (floor)
        {
            case 1:
                AddButton = ui.FirstAddB;
                break;
            case 2:
                AddButton = ui.SecondAddB;
                break;
            case 3:
                AddButton = ui.ThirdAddB;
                break;
            case 4:
                AddButton = ui.FourthAddB;
                break;
            default:
                return;
        }
        //Добавляем новую ячейку в ui на BuildPanel.Content
        Vector3 RoomBlockPos = AddButton.GetComponent<RectTransform>().localPosition;
        RoomBlock NewRB = Instantiate(RoomBlockInst, ui.ContentPanel.transform);
        NewRB.GetComponent<RectTransform>().localPosition = RoomBlockPos;
        RoomBlockPos.x += RoomBlockSize + RoomBlockShift;
        AddButton.GetComponent<RectTransform>().localPosition = RoomBlockPos;
        //Добавляем новую комнату на уровень
        int CurrentRoom = get_num_of_rooms(floor) + 1;
        Vector3 RoomPos;
        RoomPos.x = CurrentRoom * RoomWidth;
        RoomPos.y = (floor - 1) * RoomHeight;
        RoomPos.z = 0;
        Instantiate(RoomInst, RoomPos, new Quaternion());
        RoomTable[floor-1, CurrentRoom] = 1;
    }

    //Получить количество комнат на выбранном этаже
    private int get_num_of_rooms(int floor)
    {
        int NumOfRooms = 0;
        for(int i = 0; i<MaxRoomsOnFloor; i++)
        {
            NumOfRooms += RoomTable[floor - 1, i];
        }
        return NumOfRooms;
    }

    //Улучшить выбранную комнату до выбранного уровня 
    public void upgrade_room(Room SelectedRoom, string Upgrade)
    {
        switch(Upgrade)
        {
            case "Stairs":
                SelectedRoom = new Stairs();
                break;
        }
    }

}
