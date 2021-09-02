using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoomButton : MonoBehaviour
{
    private LevelManager _LM;
    public int Floor;
    public PriceForBuild Price;

    private void Start()
    {
        _LM = Object.FindObjectOfType<LevelManager>();
        Price = transform.Find("PriceForBuild").GetComponent<PriceForBuild>();

        switch (transform.name)
        {
            case "FirstAddButton":
                Floor = 0;
                break;
            case "SecondAddButton":
                Floor = 1;
                break;
            case "ThirdAddButton":
                Floor = 2;
                break;
            case "FourthAddButton":
                Floor = 3;
                break;
        }
    }

    //Если количество комнат на этаже максимальное, то скрываем кнопку
    public void check_num_of_rooms()
    {
        int NumOfRoomsOnFloor = _LM.get_num_of_rooms_on_floor(Floor);
        if(NumOfRoomsOnFloor == LevelManager.MaxRoomsOnFloor)
        {
            this.gameObject.SetActive(false);
        }
    }

}
