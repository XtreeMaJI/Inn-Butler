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
        /*Vector3 RoomPos = new Vector3(0f, 0f, 0f);
        Instantiate(RoomInst, RoomPos, new Quaternion());
        RoomPos.Set(0f, RoomHeight, 0f);
        Instantiate(RoomInst, RoomPos, new Quaternion());
        RoomPos.Set(RoomWidth, 0f, 0f);
        Instantiate(StairsInst, RoomPos, new Quaternion()).init(_LM);
        RoomPos.Set(RoomWidth, RoomHeight, 0f);
        Instantiate(StairsInst, RoomPos, new Quaternion()).init(_LM);*/

    }

}
