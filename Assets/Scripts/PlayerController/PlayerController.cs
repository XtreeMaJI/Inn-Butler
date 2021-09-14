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

    private Animator _Animator;

    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();

        this.transform.position = new Vector3(-0.75f, 0f, -1f);

        FollowingVisitor = null;

        PlayerState = StateOfPlayer.Common;

        _Animator = GetComponent<Animator>();
    }

    void Update()
    {
        float dir = Input.GetAxis("Horizontal");
        switch (PlayerState)
        {
            case StateOfPlayer.Common:
                set_dir(dir);
                dir = Mathf.Abs(dir);
                _Animator.SetFloat("Speed", Mathf.Abs(dir));
                _rb.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0f, 0f));
                break;
            case StateOfPlayer.Cleaning:
                break;
            case StateOfPlayer.Cooking:
                break;
            case StateOfPlayer.Carrying:
                set_dir(dir);
                dir = Mathf.Abs(dir);
                _Animator.SetFloat("Speed", Mathf.Abs(dir));
                _rb.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0f, 0f));
                break;
        }
        set_CamPos();
    }

    public void set_PlayerState(StateOfPlayer NewState)
    {
        PlayerState = NewState;

        _Animator.SetBool("IsCleaning", false);
        _Animator.SetBool("IsCooking", false);
        _Animator.SetBool("IsCarry", false);
        switch (NewState)
        {
            case StateOfPlayer.Common:
                break;
            case StateOfPlayer.Cleaning:
                _Animator.SetBool("IsCleaning", true);
                break;
            case StateOfPlayer.Cooking:
                _Animator.SetBool("IsCooking", true);
                break;
            case StateOfPlayer.Carrying:
                _Animator.SetBool("IsCarry", true);
                break;
        }
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

    private void set_CamPos()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        cam.transform.position = new Vector3(x, y, -10f); 
    }

    //Направление взгляда персонажа
    private void set_dir(float dir)
    {
        if (dir > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

}
