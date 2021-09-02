using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    private const int WORKERS_RANGE_OF_REACH = 4; //Какое количество комнат справа и слева будет обслуживаться работинком

    private List<LivingRoom> CleanQueue;
    private List<LivingRoom> FoodQueue;
    private List<LivingRoom> WineQueue;

    private void Start()
    {
        CleanQueue = new List<LivingRoom>();
        FoodQueue = new List<LivingRoom>();
        WineQueue = new List<LivingRoom>();
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
        if(RoomForClean == null)
        {
            return;
        }

        for(int i=0; i<CleanQueue.Count; i++)
        {
            if(ReferenceEquals(CleanQueue[i], RoomForClean))
            {
                CleanQueue.RemoveAt(i);
            }
        }
    }

    //WorkerRoom - комната работника, который вызывает эту функцию
    public LivingRoom get_room_from_queue(Room WorkerRoom)
    {
        List<LivingRoom> Queue = new List<LivingRoom>();
        if (WorkerRoom.RoomType == LevelManager.TypeOfRoom.StaffRoom)
        {
            Queue = new List<LivingRoom>(CleanQueue);
        }
        if (WorkerRoom.RoomType == LevelManager.TypeOfRoom.Kitchen)
        {
            Queue = new List<LivingRoom>(FoodQueue);
        }
        if (WorkerRoom.RoomType == LevelManager.TypeOfRoom.Bar)
        {
            Queue = new List<LivingRoom>(WineQueue);
        }

        for (int i=0; i<Queue.Count; i++)
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

        if((NumOfRoomFirst + WORKERS_RANGE_OF_REACH >= NumOfRoomSecond && NumOfRoomFirst <= NumOfRoomSecond) ||
           (NumOfRoomFirst - WORKERS_RANGE_OF_REACH <= NumOfRoomSecond && NumOfRoomFirst >= NumOfRoomSecond))
        {
            return true;
        }
        return false;
    }

    public void add_room_in_FoodQueue(LivingRoom RoomForFood)
    {
        if (FoodQueue.Contains(RoomForFood))
        {
            return;
        }
        FoodQueue.Add(RoomForFood);
    }

    public void delete_room_from_FoodQueue(LivingRoom RoomForFood)
    {
        if(RoomForFood == null)
        {
            return;
        }

        for (int i = 0; i < FoodQueue.Count; i++)
        {
            if (ReferenceEquals(FoodQueue[i], RoomForFood))
            {
                FoodQueue.RemoveAt(i);
            }
        }
    }

    public void add_room_in_WineQueue(LivingRoom RoomForWine)
    {
        if (WineQueue.Contains(RoomForWine))
        {
            return;
        }
        WineQueue.Add(RoomForWine);
    }

    public void delete_room_from_WineQueue(LivingRoom RoomForWine)
    {
        if (RoomForWine == null)
        {
            return;
        }

        for (int i = 0; i < WineQueue.Count; i++)
        {
            if (ReferenceEquals(WineQueue[i], RoomForWine))
            {
                WineQueue.RemoveAt(i);
            }
        }
    }

}
