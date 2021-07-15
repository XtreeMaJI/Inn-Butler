using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    //Лестницы, соединённые с текущей
    public Stairs UpperStair;
    public Stairs LowerStair;

    public Vector3 MidPos; //Середина лестницы

    private void Start()
    {
        UpperStair = null;
        LowerStair = null;

        MidPos = transform.position;

        connect_stairs();
    }

    //Проверяем, есть ли сверху и снизу другие лестницы и соединяем, если есть 
    void connect_stairs()
    {
        Stairs[] ListOfStairs = FindObjectsOfType<Stairs>();
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        foreach (Stairs stair in ListOfStairs)
        {
            if(stair.transform.position.x == x)
            {
                if(stair.transform.position.y > y)
                {
                    UpperStair = stair;
                    stair.LowerStair = this.GetComponent<Stairs>();
                }
                else if(stair.transform.position.y < y)
                {
                    LowerStair = stair;
                    stair.UpperStair = this.GetComponent<Stairs>();
                }
            }
        }
    }

    public void climb_up(GameObject Person)
    {
        if (UpperStair != null)
        {
            Person.transform.SetPositionAndRotation(UpperStair.MidPos, new Quaternion());
            if(Person.tag == "Player")
            {
                Person.GetComponent<PlayerController>().ActiveStair = UpperStair;
            }
        }
    }

    public void climb_down(GameObject Person)
    {
        if (LowerStair != null)
        {
            Person.transform.SetPositionAndRotation(LowerStair.MidPos, new Quaternion());
            if (Person.tag == "Player")
            {
                Person.GetComponent<PlayerController>().ActiveStair = LowerStair;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            controller.ActiveStair = GetComponent<Stairs>();
            controller.ui.toggle_arrows();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            if (controller.ActiveStair == GetComponent<Stairs>())
            {
                controller.ActiveStair = null;
            }
            controller.ui.toggle_arrows();
        }
    }

}
