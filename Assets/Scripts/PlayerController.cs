using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Обрабатываем нажатия клавиш, передаём параметры в аниматор и привязываем камеру к игроку*/

public class PlayerController : BaseType
{
    public GameObject cam; //Главная камера
    
    private Rigidbody2D _rb;

    public float speed;

    public UI ui;

    public Visitor FollowingVisitor { get; set; }

    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();

        this.transform.position = new Vector3(-0.75f, 0f, -1f);

        FollowingVisitor = null;
    }

    void Update()
    {
        float dir = Input.GetAxis("Horizontal");
        _rb.transform.Translate(new Vector3(dir*speed*Time.deltaTime, 0f, 0f));

        change_cam_pos();
    }

    private void change_cam_pos()
    {
        Vector3 NewPos = this.transform.position;
        NewPos.z = cam.transform.position.z;
        cam.transform.SetPositionAndRotation(NewPos, new Quaternion());
    }

}
