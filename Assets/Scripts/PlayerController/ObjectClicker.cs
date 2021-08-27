using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    private float _MaxRayDistance = 100f;

    private Transform _PosForCarry;

    private const float PICK_UP_X_RANGE = 0.3f;
    private const float PICK_UP_Y_RANGE = 0.2f;

    private PlayerController _PC;

    private void Start()
    {
        _PosForCarry = transform.Find("PosForCarry");
        _PC = GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 MousePos = Input.mousePosition;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);
            RaycastHit2D Hit = Physics2D.Raycast(MousePos, Vector2.zero, _MaxRayDistance);
            if (Hit)
            {
                Carryable carryable = Hit.collider.GetComponent<Carryable>();
                if(carryable == null)
                {
                    return;
                }
                if(is_player_near_item(carryable.transform) &&
                   _PC.get_PlayerState() == PlayerController.StateOfPlayer.Common)
                {
                    _PC.set_PlayerState(PlayerController.StateOfPlayer.Carrying);
                    _PC.set_Item(carryable);
                    carryable.grab(_PosForCarry);
                }
            }
        }
    }

    private bool is_player_near_item(Transform ItemTransform)
    {
        float ItemX = ItemTransform.position.x;
        float ItemY = ItemTransform.position.y;
        float PlayerX = transform.position.x;
        float PlayerY = transform.position.y;

        //Если по y находимся не на том этаже, то возвращаем false
        if ((PlayerY + PICK_UP_Y_RANGE >= ItemY && PlayerY < ItemY) ||
           (PlayerY - PICK_UP_Y_RANGE <= ItemY && PlayerY > ItemY))
        {
            
        }
        else
        {
            return false;
        }

        if ((PlayerX + PICK_UP_X_RANGE >= ItemX && PlayerX < ItemX) ||
           (PlayerX - PICK_UP_X_RANGE <= ItemX && PlayerX > ItemX))
        {
            return true;
        }
        return false;
    }

}
