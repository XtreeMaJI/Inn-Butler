using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public Room RoomInst;
    public Stairs StairsInst;

    private float RoomWidth;
    private float RoomHeight;

    private LevelManager _LM;

    private void Start()
    {
        RoomWidth = RoomInst.GetComponent<BoxCollider2D>().size.x;
        RoomHeight = RoomInst.transform.Find("HeightMeasurment").GetComponent<BoxCollider2D>().size.y;
        _LM = GetComponent<LevelManager>();
        _LM.RoomHeight = RoomHeight;
        _LM.RoomWidth = RoomWidth;

        //Создаём первые 4 комнаты
        _LM.add_room(1);
        _LM.add_room(1);
        _LM.add_room(2);
        _LM.add_room(2);
        //Две из них делаем лестницами
        Room[] StartingListOfRooms = FindObjectsOfType<Room>();
        foreach(Room room in StartingListOfRooms)
        {
            if(room.transform.position.x == 2 * RoomWidth)
            {
                _LM.upgrade_room(room, LevelManager.TypeOfRoom.Stairs);
            }
        }

    }

}
