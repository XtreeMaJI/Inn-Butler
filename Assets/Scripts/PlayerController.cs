﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Обрабатываем нажатия клавиш, передаём параметры в аниматор и привязываем камеру к игроку*/

public class PlayerController : MonoBehaviour
{
    public GameObject cam; //Главная камера
    
    private Rigidbody2D _rb;
    private Transform _trans; //Трансформ игрока

    public float speed;


    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
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