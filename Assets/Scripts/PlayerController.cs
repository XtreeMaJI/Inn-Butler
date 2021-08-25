using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Обрабатываем нажатия клавиш, передаём параметры в аниматор и привязываем камеру к игроку*/

public class PlayerController : BaseType
{
    public enum StateOfPlayer: int
    {
        Common = 0,
        Cleaning = 1
    }

    public GameObject cam; //Главная камера
    
    private Rigidbody2D _rb;

    public float speed;

    public UI ui;

    public Visitor FollowingVisitor { get; set; }

    private StateOfPlayer PlayerState;

    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();

        this.transform.position = new Vector3(-0.75f, 0f, -1f);

        FollowingVisitor = null;

        PlayerState = StateOfPlayer.Common;
    }

    void Update()
    {
        switch (PlayerState)
        {
            case StateOfPlayer.Common:
                float dir = Input.GetAxis("Horizontal");
                _rb.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0f, 0f));
                break;
            case StateOfPlayer.Cleaning:
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

}
