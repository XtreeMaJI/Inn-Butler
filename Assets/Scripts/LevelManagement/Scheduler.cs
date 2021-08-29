using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    private const int WORKERS_RANGE_OF_REACH = 4; //Какое количество комнат справа и слева будет обслуживаться работинком

    private List<LivingRoom> CleanQueue;

    private void Start()
    {
        CleanQueue = new List<LivingRoom>();
    }

    public void add_room_in_CleanQueue(LivingRoom RoomForClean)
    {
        if(CleanQueue.Contains(RoomForClean))
        {
            return;
        }
        CleanQueue.Add(RoomForClean);
    }

    public void delete_room_from_CleanQueue(LivingRoom RoomForClean)
    {
        for(int i=0; i<CleanQueue.Count; i++)
        {
            if(ReferenceEquals(CleanQueue[i], RoomForClean))
            {
                CleanQueue.RemoveAt(i);
            }
        }
    }

    //WorkerRoom - комната работника, который вызывает эту функцию
    public LivingRoom get_closest_room_from_queue(Room WorkerRoom)
    {
        List<LivingRoom> Queue = new List<LivingRoom>();
        if (WorkerRoom.RoomType == LevelManager.TypeOfRoom.StaffRoom)
        {
            Queue = new List<LivingRoom>(CleanQueue);
        }

        for(int i=0; i<Queue.Count; i++)
        {
            if(is_rooms_in_range_of_reach(WorkerRoom, Queue[i]))
            {
                return Queue[i];
            }
        }
        return null;
    }

    private bool is_rooms_in_range_of_reach(Room FirstRoom, Room SecondRoom)
    {
        int NumOfRoomFirst = FirstRoom.PosInTable.NumOfRoom;
        int NumOfRoomSecond = SecondRoom.PosInTable.NumOfRoom;

        if((NumOfRoomFirst + WORKERS_RANGE_OF_REACH > NumOfRoomSecond && NumOfRoomFirst < NumOfRoomSecond) ||
           (NumOfRoomFirst - WORKERS_RANGE_OF_REACH < NumOfRoomSecond && NumOfRoomFirst > NumOfRoomSecond))
        {
            return true;
        }
        return false;
    }

}
