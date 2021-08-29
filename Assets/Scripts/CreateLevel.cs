﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public Room RoomInst;
    public Stairs StairsInst;
    public TavernExit TavernExitInst;

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
        Room FirstRoom = _LM.upgrade_room(_LM.add_room(0), LevelManager.TypeOfRoom.Reception);
        _LM.add_room(0);
        _LM.upgrade_room(_LM.add_room(0), LevelManager.TypeOfRoom.TraderRoom);
        _LM.upgrade_room(_LM.add_room(1), LevelManager.TypeOfRoom.Room);
        _LM.add_room(1);
        _LM.upgrade_room(_LM.add_room(1), LevelManager.TypeOfRoom.TraderRoom);

        //Создаём выход из таверны
        float x = FirstRoom.transform.position.x - RoomWidth;
        float y = FirstRoom.transform.position.y;
        _LM.ExitFromTavern = Instantiate(TavernExitInst, new Vector3(x, y, 0f), new Quaternion());
    }

}
