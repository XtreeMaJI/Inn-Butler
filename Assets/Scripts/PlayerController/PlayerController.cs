using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Обрабатываем нажатия клавиш, передаём параметры в аниматор и привязываем камеру к игроку*/

public class PlayerController : BaseType
{
    public enum StateOfPlayer: int
    {
        Common = 0,
        Cleaning = 1,
        Cooking = 2,
        Carrying = 3
    }

    public GameObject cam; //Главная камера
    
    private Rigidbody2D _rb;

    private float speed = 1f;

    public UI ui;

    public Visitor FollowingVisitor { get; set; }

    [SerializeField]private StateOfPlayer PlayerState;
    private Carryable _Item;

    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();

        this.transform.position = new Vector3(-0.75f, 0f, -1f);

        FollowingVisitor = null;

        PlayerState = StateOfPlayer.Common;
    }

    void Update()
    {
        float dir = Input.GetAxis("Horizontal");
        switch (PlayerState)
        {
            case StateOfPlayer.Common:
                _rb.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0f, 0f));
                break;
            case StateOfPlayer.Cleaning:
                break;
            case StateOfPlayer.Cooking:
                break;
            case StateOfPlayer.Carrying:
                _rb.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0f, 0f));
                break;
        }

        change_cam_pos();
    }

    private void change_cam_pos()
    {
        Vector3 NewPos = this.transform.position;
        NewPos.z = cam.transform.position.z;
        cam.transform.SetPositionAndRotation(NewPos, new Quaternion());
    }

    public void set_PlayerState(StateOfPlayer NewState)
    {
        PlayerState = NewState;
    }

    public StateOfPlayer get_PlayerState()
    {
        return PlayerState;
    }

    public Carryable get_Item()
    {
        return _Item;
    }

    public void set_Item(Carryable NewItem)
    {
        _Item = NewItem;
    }

}
