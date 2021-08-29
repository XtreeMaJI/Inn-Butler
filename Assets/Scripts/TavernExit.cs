using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Наследуем от Room, так как всем персонажам нужен объект этого типа для указания точки движения
public class TavernExit : Room
{
    protected new void Awake()
    {
        CurFloor = 0;
    }

    protected override void enable_buttons()
    {

    }

    protected override void disable_buttons()
    {

    }
}

