using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public Room RoomInst;
    public Stairs StairsInst;

    private float RoomWidth;
    private float RoomHeight;

    private void Start()
    {
        RoomWidth = RoomInst.GetComponent<BoxCollider2D>().size.x;
        RoomHeight = RoomInst.transform.Find("HeightMeasurment").GetComponent<BoxCollider2D>().size.y;

        //Создаём первые 4 комнаты
        Vector3 RoomPos = new Vector3(0f, 0f, 0f);
        Instantiate(RoomInst, RoomPos, new Quaternion());
        RoomPos.Set(0f, RoomHeight, 0f);
        Instantiate(RoomInst, RoomPos, new Quaternion());
        RoomPos.Set(RoomWidth, 0f, 0f);
        Instantiate(StairsInst, RoomPos, new Quaternion());
        RoomPos.Set(RoomWidth, RoomHeight, 0f);
        Instantiate(StairsInst, RoomPos, new Quaternion());
    }

}
